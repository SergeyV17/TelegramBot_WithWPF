using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using Telegram.Bot;
using ApiAiSDK;
using Telegram.Bot.Types.Enums;

namespace Homework_09_Vlasenko.S.A
{
    public class Program
    {
        /// <summary>
        /// Поле бота
        /// </summary>
        public static TelegramBotClient bot;

        /// <summary>
        /// Поле интерфейса ИИ
        /// </summary>
        private static ApiAi apiAi;

        /// <summary>
        /// Метод получения информации при нажатии кнопок меню
        /// </summary>
        /// <param name="sender">отправитель(кто нажал кнопку)</param>
        /// <param name="e">аргументы приходящие при нажатии кнопки меню</param>
        public static void CallbackQueryListener(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            string buttonText = e.CallbackQuery.Data; // Название нажатой кнопки
            string nameFrom = $"{e.CallbackQuery.From.FirstName} {e.CallbackQuery.From.LastName}"; // Кто нажал кнопку
            Console.WriteLine($"{nameFrom} нажал кнопку {buttonText}"); // Вывод в консоль информации взаимодействия пользователя с кнопкой

            if (buttonText == "My photos")
                Send.SendPhotos(bot,e);  // Отправка фото
            else if (buttonText == "My favorite song")
                Send.SendAudio(bot,e);  // Отправка аудио
            else if (buttonText == "About my city")
                Send.SendDocument(bot,e); // Отправка документа
        }

        /// <summary>
        /// Метод получения информации при получении ботом сообщений
        /// </summary>
        /// <param name="sender">отправитель</param>
        /// <param name="e">аргументы приходящего сообщения</param>
        public static async void MessageListener(object sender, Telegram.Bot.Args.MessageEventArgs e )
        {
            // Реагирование бота на разные типы файлов отправляемых боту
            if (e.Message == null )
                return;
            else if (e.Message.Type == MessageType.Document)
                DownLoad.DownloadDocument(bot,e);
            else if (e.Message.Type == MessageType.Audio)
                DownLoad.DownloadAudio(bot,e);
            else if (e.Message.Type == MessageType.Photo)
                DownLoad.DownloadPhoto(bot,e);
            else if (e.Message.Type == MessageType.Sticker)
                await bot.SendTextMessageAsync(e.Message.Chat.Id, "Очень смешно! Хочу такой же стикер.");
            else
            {
                // Если это простое текстовое сообщение
                string name = $"{e.Message.From.FirstName} {e.Message.From.LastName}";
                //Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: {name} отправил сообщение: {e.Message.Text}");

                switch (e.Message.Text)
                {
                    case "/start": // запуск бота
                        Command.StartCommand(bot,e);
                        break;

                    case "/menu": // отправка меню
                       Command.MenuCommand(bot,e);
                        break;

                    case "/description": // описание
                        Command.DescriptionCommand(bot,e);
                        break;

                    default:
                        // В случае отправки боту произвольно текста, подключена библиотека ApiAi с ИИ для ответа пользователю
                        try
                        {
                            var responce = apiAi.TextRequest(e.Message.Text); // Сохраненить в переменную "responce" текст, который отправил пользователь
                            string answer = responce.Result.Fulfillment.Speech; // Подобрать ответ из списка ответов ИИ на текст пользователя

                            if (answer == "хаха")
                            {
                                Send.SendSticker(bot, e); // При отправке "ха-ха" пользователю отправляется стикер
                                break;
                            }
                            else if (answer == "")
                            {
                                // При отправке сообщения, который не распознаёт ИИ
                                await bot.SendTextMessageAsync(e.Message.Chat.Id, "Я тебя не понял");
                                break;
                            }
                            await bot.SendTextMessageAsync(e.Message.Chat.Id, answer); // Отправит подобранный ответ
                            break;
                        }
                        catch (Exception)
                        {
                            try
                            {
                                await bot.SendTextMessageAsync(e.Message.Chat.Id, "Я не могу отвечать на такой длинный текст, лимит символов - 256"); // Сообение об ошибке

                            }
                            catch (Exception exception)
                            {
                                Console.WriteLine(exception);
                                return;
                            }
                            break;
                        }
                       
                }
            }
        }

        /// <summary>
        /// Инициализация бота и ИИ
        /// </summary>
        public static void BotInitialization()
        {
            string token = File.ReadAllText(@"C:\Users\Sergey\Desktop\Программирование\MyToken.txt");
            bot = new TelegramBotClient(token); // инициализация бота

            AIConfiguration config = new AIConfiguration("6dff9142fdb3499c937673175c0c52b6", SupportedLanguage.Russian); // конфигурация ИИ
            apiAi = new ApiAi(config); // инициализация ИИ
        }

        static void Main()
        {
            BotInitialization();

            var myBot = bot.GetMeAsync().Result; // Получение информации о боте ( использую для того чтобы проверить связь, если подключение прошло)
            Console.WriteLine($"bot name: {myBot.FirstName} \nstatus: ready");

            bot.OnMessage += MessageListener; // получение сообщений ботом
            bot.OnCallbackQuery += CallbackQueryListener; // получение информации ботом по нажатой кнопки меню

            bot.StartReceiving(); // начать приём данных
        }
    }
}

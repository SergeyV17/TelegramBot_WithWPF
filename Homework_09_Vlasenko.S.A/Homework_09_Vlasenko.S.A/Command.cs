using System;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Homework_09_Vlasenko.S.A
{
    /// <summary>
    /// Класс команд меню
    /// </summary>
    static class Command
    {
        /// <summary>
        /// Команда старт
        /// </summary>
        /// <param name="bot">бот</param>
        /// <param name="e">аргументы приходящего сообщения</param>
        public static async void StartCommand(TelegramBotClient bot, Telegram.Bot.Args.MessageEventArgs e)
        {
            string text =
                "Список команд:\n" +
                "/start - запуск бота\n" +
                "/menu - меню\n" +
                "/description - описание";
            try
            {
                await bot.SendTextMessageAsync(e.Message.Chat.Id, text);

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return;
            }
        }

        /// <summary>
        /// Команда вывода меню дополнительных команд
        /// </summary>
        /// <param name="bot">бот</param>
        /// <param name="e">аргументы приходящего сообщения</param>
        public static async void MenuCommand(TelegramBotClient bot, Telegram.Bot.Args.MessageEventArgs e)
        {
            ///Массив из кнопок, создал массив массивов чтобы кнопки стояли столбиком
            var menu = new InlineKeyboardMarkup(new[]
            {
                new []
                {
                    // Кнопка ссылки на сайт
                    InlineKeyboardButton.WithUrl("My page","https://ru.wikipedia.org/wiki/Томми_Версетти"),
                },
                new []
                {
                    // Кнопка отправки пользователю аудио
                    InlineKeyboardButton.WithCallbackData("My favorite song"),
                },
                new []
                {
                    // Кнопка отправки пользователю фотографий
                    InlineKeyboardButton.WithCallbackData("My photos"),
                },
                new []
                {
                    // Кнопка отправки пользователю документа
                    InlineKeyboardButton.WithCallbackData("About my city")
                }
            });
            // Отправка сообщения при выводе меню
            try
            {
                await bot.SendTextMessageAsync(e.Message.Chat.Id, "Выберите пункт меню: ", replyMarkup: menu);

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return;
            }
        }

        /// <summary>
        /// Команда вывода описания
        /// </summary>
        /// <param name="bot">бот</param>
        /// <param name="e">аргументы приходящего сообщения</param>
        public static async void DescriptionCommand(TelegramBotClient bot, Telegram.Bot.Args.MessageEventArgs e)
        {
            string description =
                "Данный бот создан для обучения, он умеет с вами произвольно общаться, " +
                "сохранять данные на диск, отправлять документ, фото, аудио и стикеры." +
                "Для того чтобы начать взаимодействие с ботом нажмите / start.";

            //Отправка сообщения с описанием пользователю
            await bot.SendTextMessageAsync(e.Message.Chat.Id, description);
        }
    }
}

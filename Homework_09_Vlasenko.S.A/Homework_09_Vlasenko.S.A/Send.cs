using System;
using System.IO;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace Homework_09_Vlasenko.S.A
{
    /// <summary>
    /// Класс отправки файлов от бота пользователю
    /// </summary>
    public static class Send
    {
        /// <summary>
        /// Отправка нескольких фото
        /// </summary>
        /// <param name="bot">бот</param>
        /// <param name="e">аргументы приходящие при нажатии кнопки меню</param>
        public async static void SendPhotos(TelegramBotClient bot, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            // Ссылки на фото заранее заготовлены в классе
            await bot.SendMediaGroupAsync(new[]
            {
                new InputMediaPhoto("https://i.playground.ru/p/Y3nwcgYgv7B6GQDifA8QAg.jpeg")
                {
                    Caption = "Мои лучшие фото" //Описание, которое выводится при отправке массива фото в чат
                },
                new InputMediaPhoto("https://i.pinimg.com/originals/b1/40/cb/b140cbbb420afc2cecb18d80e34e8389.jpg"),
                new InputMediaPhoto("https://i.redd.it/6jsfs0pwtve31.jpg"),
                new InputMediaPhoto("https://i.ytimg.com/vi/TWUcdhur1zc/hqdefault.jpg"),
            }, e.CallbackQuery.From.Id);
        }

        /// <summary>
        /// Отправка аудио
        /// </summary>
        /// <param name="bot">бот</param>
        /// <param name="e">аргументы приходящие при нажатии кнопки меню</param>
        public async static void SendAudio(TelegramBotClient bot, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            //Аудио заранее заготовлено. Данное аудио будет скачано с компьютера через поток
            using (FileStream fs = System.IO.File.OpenRead(@"ToSend\ViceTheme.mp3"))
            {
                // Уведомление пользователя о том, что он нажал кнопку меню
                await bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, "Вы нажали кнопку - My favorite song");

                // Отправка файла пользователю в чат
                await bot.SendAudioAsync(e.CallbackQuery.From.Id, new InputOnlineFile(fs, "ViceTheme.mp3"),
                    "Моя любимая песня");
            }
            await bot.SendAudioAsync(e.CallbackQuery.From.Id, "https://zaycev.net/musicset/dl/f328243287220579dd2b8b0f8f9c9b84/3242198.json?spa=false", "Также песня для авто");
        }

        /// <summary>
        /// Отправка документа
        /// </summary>
        /// <param name="bot">бот</param>
        /// <param name="e">аргументы приходящие при нажатии кнопки меню</param>
        public async static void SendDocument(TelegramBotClient bot, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            //Документ заранее заготовлен. Данный документ будет скачан с компьютера через поток
            using (FileStream fs = System.IO.File.OpenRead(@"ToSend\ViceCityTown.rtf"))
            {
                // Уведомление пользователя о том, что он нажал кнопку меню
                await bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, "Вы нажали кнопку - About my city");

                // Отправка файла пользователю в чат
                await bot.SendDocumentAsync(e.CallbackQuery.From.Id, new InputOnlineFile(fs, "ViceCityTown.rtf"),
                    "Прошу ознакомиться с документом");
            }
        }

        /// <summary>
        /// Отправка стикера
        /// </summary>
        /// <param name="bot">бот</param>
        /// <param name="e">аргументы приходящего сообщения</param>
        public async static void SendSticker(TelegramBotClient bot, Telegram.Bot.Args.MessageEventArgs e)
        {
            //Стикер заранее заготовлен. Данный стикен будет скачан с компьютера через поток
            using (FileStream fs = System.IO.File.OpenRead(@"ToSend\AnimatedSticker.tgs"))
            {
                //Отправка стикера в чат
                await bot.SendStickerAsync(e.Message.Chat.Id, new InputOnlineFile(fs, "AnimatedSticker.tgs"));
            }
        }

        /// <summary>
        /// Отправка сообщения через UI WPF
        /// </summary>
        /// <param name="Text">Текст сообщения</param>
        /// <param name="Id">ID чата</param>
        /// <param name="bot">бот</param>
        public static async void SendMessage(string Text, string Id, TelegramBotClient bot)
        {
            if (String.IsNullOrEmpty(Text) || String.IsNullOrEmpty(Id))
                return;
            else
            {
                long id = Convert.ToInt64(Id);
                await bot.SendTextMessageAsync(id, Text);
            }

        }

        /// <summary>
        /// Отправка файла через UI WPF
        /// </summary>
        /// <param name="FilePath">Путь к файлу</param>
        /// <param name="FileName">Имя файла</param>
        /// <param name="Id">ID чата</param>
        /// <param name="bot">бот</param>
        public static async void SendFile(string FilePath, string FileName, string Id, TelegramBotClient bot)
        {
            if (String.IsNullOrEmpty(FilePath) || String.IsNullOrEmpty(Id))
                return;
            else
            {
                using (FileStream fs = System.IO.File.OpenRead(FilePath))
                {
                    long id = Convert.ToInt64(Id);
                    await bot.SendDocumentAsync(Id, new InputOnlineFile(fs, FileName));
                }
            }
        }
    }
}

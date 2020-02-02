using System.IO;
using  System;
using Telegram.Bot;

namespace Homework_09_Vlasenko.S.A
{
    /// <summary>
    /// Класс загрузки файлов отправляемых боту от пользователей
    /// </summary>
    static class DownLoad
    {
        /// <summary>
        /// Скачать
        /// </summary>
        /// <param name="fileId">ID файла</param>
        /// <param name="path">путь к файлу</param>
        /// <param name="bot">бот</param>
        static async void Download(string fileId, string path, TelegramBotClient bot)
        {
            var file = await bot.GetFileAsync(fileId); // Получить файл из сообщения по id
            FileStream fs = new FileStream(path, FileMode.Create); // Создать поток для скачивания
            await bot.DownloadFileAsync(file.FilePath, fs); // Используя поток скачать файл
            fs.Close();

            fs.Dispose(); // Закрыть поток, освободить ресурсы
        }

        /// <summary>
        /// Скачать документ
        /// </summary>
        /// <param name="bot">бот</param>
        /// <param name="e">аргументы приходящего сообщения</param>
        public static async void DownloadDocument(TelegramBotClient bot, Telegram.Bot.Args.MessageEventArgs e)
        {
            //-------Вывод в консоль информации об отправленном файле, тип, кем отправлен.
            string name = $"{e.Message.From.FirstName} {e.Message.From.LastName}";
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: {name} отправил: {e.Message.Type.ToString()}");

            //------Скачивание файла, уведомление об успешном скачивании в консоль
            Download(e.Message.Document.FileId, @"ToDownload\" + e.Message.Document.FileName, bot);
            Console.WriteLine($"Файл: {e.Message.Document.FileName} сохранен");

            //------Отправка сообщения от бота пользователю после скачивания файла
            await bot.SendTextMessageAsync(e.Message.Chat.Id, $"Файл {e.Message.Document.FileName} успешно сохранен в папку ToDownload");
        }

        /// <summary>
        /// Скачать аудио
        /// </summary>
        /// <param name="bot">бот</param>
        /// <param name="e">аргументы приходящего сообщения</param>
        public static async void DownloadAudio(TelegramBotClient bot, Telegram.Bot.Args.MessageEventArgs e)
        {
            //-------Вывод в консоль информации об отправленном файле, тип, кем отправлен.
            string name = $"{e.Message.From.FirstName} {e.Message.From.LastName}";
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: {name} отправил: {e.Message.Type.ToString()}");

            //------Скачивание файла, уведомление об успешном скачивании в консоль
            Download(e.Message.Audio.FileId, @"ToDownload\" + e.Message.Audio.Performer + "-" + e.Message.Audio.Title + ".mp3", bot);
            Console.WriteLine($"Аудио: {e.Message.Audio.Performer} {e.Message.Audio.Title} сохранено");

            //------Отправка сообщения от бота пользователю после скачивания файла
            await bot.SendTextMessageAsync(e.Message.Chat.Id, $"Аудио {e.Message.Audio.Performer} - {e.Message.Audio.Title}.mp3 успешно сохранено в папку ToDownload");
        }

        /// <summary>
        /// Скачать фото
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="e"></param>
        public static async void DownloadPhoto(TelegramBotClient bot, Telegram.Bot.Args.MessageEventArgs e)
        {
            //-------Вывод в консоль информации об отправленном файле, тип, кем отправлен.
            string name = $"{e.Message.From.FirstName} {e.Message.From.LastName}";
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: {name} отправил: {e.Message.Type.ToString()}");

            //------Скачивание файла, уведомление об успешном скачивании в консоль
            Download(e.Message.Photo[e.Message.Photo.Length - 1].FileId, @"ToDownload\" + (e.Message.Photo[e.Message.Photo.Length - 1].FileId) + ".jpg", bot);
            Console.WriteLine($"Фото: {e.Message.Photo[e.Message.Photo.Length - 1].FileId} сохранено");

            //------Отправка сообщения от бота пользователю после скачивания файла
            await bot.SendTextMessageAsync(e.Message.Chat.Id, $"Фото {e.Message.Photo[e.Message.Photo.Length - 1].FileId}.jpg успешно сохранено в папку ToDownload");
        }
    }
}

using System;
using System.Collections.ObjectModel;
using Telegram.Bot.Args;
using Homework_09_Vlasenko.S.A;
using Telegram.Bot.Types.Enums;
using System.Collections.Generic;

namespace Homework_10_VlasenkoS.A
{
    class TelegramMessageClient
    {
        private MainWindow w; // Поле основного окна

        public ObservableCollection<MessageLog>BotMessageLog { get; set; } // Коллекция для хранения истории сообщений
        public HashSet<ClientBase>BotClientBase { get; set; } // Коллекция для хранения клиенсткой базы

        /// <summary>
        /// Метод обрабатывающий полученные ботом сообщения
        /// </summary>
        /// <param name="sender">отправитель</param>
        /// <param name="e">аргументы приходящего сообщения</param>
        private void MessageListener(object sender, MessageEventArgs e)
        {
            Program.MessageListener(sender, e);

            // Сделал в таком виде, чтобы распозновать имя чата, если боту пишут из чата
            w.Dispatcher.Invoke(() =>
            {
                if (e.Message.Chat.Title == null && e.Message.Type == MessageType.Text)
                {
                    BotMessageLog.Add(new MessageLog(DateTime.Now.ToLongTimeString(), e.Message.Chat.Id, e.Message.Text, e.Message.Chat.FirstName));
                    BotClientBase.Add(new ClientBase(e.Message.Chat.Id, e.Message.Chat.FirstName));
                }
                else if (e.Message.Chat.Title == null && e.Message.Type != MessageType.Text)
                {
                    BotMessageLog.Add(new MessageLog(DateTime.Now.ToLongTimeString(), e.Message.Chat.Id, e.Message.Type.ToString(), e.Message.Chat.FirstName));
                    BotClientBase.Add(new ClientBase(e.Message.Chat.Id, e.Message.Chat.FirstName));
                }
                else if (e.Message.Type != MessageType.Text)
                {
                    BotMessageLog.Add(new MessageLog(DateTime.Now.ToLongTimeString(), e.Message.Chat.Id, e.Message.Type.ToString(), e.Message.Chat.Title));
                    BotClientBase.Add(new ClientBase(e.Message.Chat.Id, e.Message.Chat.Title));
                }
                else
                {
                    BotMessageLog.Add(new MessageLog(DateTime.Now.ToLongTimeString(), e.Message.Chat.Id, e.Message.Text, e.Message.Chat.Title));
                    BotClientBase.Add(new ClientBase(e.Message.Chat.Id, e.Message.Chat.Title));
                }
            });
        }

        /// <summary>
        /// Метод обрабатывающий нажатия кнопок меню в чате с ботом
        /// </summary>
        /// <param name="sender">отправитель</param>
        /// <param name="e">аргументы приходящие при нажатии кнопки меню</param>
        private void CallbackQueryListener(object sender, CallbackQueryEventArgs e)
        {
            Program.CallbackQueryListener(sender,e);
        }

        /// <summary>
        /// Конструктор клиента
        /// </summary>
        /// <param name="w">основное окно</param>
        /// <param name="PathToken">путь к токену</param>
        public TelegramMessageClient(MainWindow w, string PathToken = @"C:\Users\Sergey\Desktop\Программирование\MyToken.txt")
        {
            //Инициализация
            this.BotMessageLog = new ObservableCollection<MessageLog>();
            this.BotClientBase = new HashSet<ClientBase>(new ClientIdComparer());
            this.w = w;

            Program.BotInitialization();

            Program.bot.OnMessage += MessageListener; // получение сообщений ботом
            Program.bot.OnCallbackQuery += CallbackQueryListener; // получение информации ботом по нажатой кнопки меню

            Program.bot.StartReceiving(); // начать приём данных
        }
    }
}

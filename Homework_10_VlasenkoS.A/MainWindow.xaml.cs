using System;
using System.IO;
using System.Windows;
using Homework_09_Vlasenko.S.A;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Homework_10_VlasenkoS.A
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TelegramMessageClient client;

        private string filePath; //Переменная для сохранения пути к файлу при выборе в openFileDialog
        private string fileName = ""; //Переменная для сохранения имени файла при выборе в openFileDialog
        private string historyPath = @"History\History.json"; //Путь к файлу сохр. истории сообщений

        /// <summary>
        /// Конструктор основного окна
        /// </summary>
        public MainWindow()
        {
            InitializeComponent(); // Иницилизация главного окна

            client = new TelegramMessageClient(this); //Инициализация TelegramMessageClient

            logList.ItemsSource = client.BotMessageLog; // Присваивание к элементам logList объектов коллекции BotMessageLog
            clientBaseLog.ItemsSource = client.BotClientBase; // Присваивание к элементам clientBaseLog объектов коллекции BotClientBase
        }

        /// <summary>
        /// Обработка кнопки "отправить сообщение"
        /// </summary>
        /// <param name="sender">отправитель</param>
        /// <param name="e">аргументы нажатия кнопки</param>
        private void btnMsgSendClick(object sender, RoutedEventArgs e)
        {
            Send.SendMessage(txtMsgSend.Text, targetSend.Text, Program.bot);
        }

        /// <summary>
        /// Обработка кнопки "Выбор файла" (Скрепка)
        /// </summary>
        /// <param name="sender">отправитель</param>
        /// <param name="e">аргументы нажатия кнопки</param>
        private void btnSelectFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                FileName.Text = openFileDialog.SafeFileName;

                fileName = openFileDialog.SafeFileName;
                filePath = openFileDialog.FileName;
            }
        }

        /// <summary>
        /// Логика отправки файла при нажатии кнопки "Отправить файл"
        /// </summary>
        /// <param name="sender">отправитель</param>
        /// <param name="e">аргументы нажатия кнопки</param>
        private void btnFileSendClick(object sender, RoutedEventArgs e)
        {
            Send.SendFile(filePath, fileName, targetSend.Text, Program.bot);
        }

        /// <summary>
        /// Обработка кнопки "Выход"
        /// </summary>
        /// <param name="sender">отправитель</param>
        /// <param name="e">аргументы нажатия кнопки</param>
        private void btnExitClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(this,
                "Вы действительно хотите выйти?",
                this.Title,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// Обработка кнопки "Сохранить историю сообщений"
        /// </summary>
        /// <param name="sender">отправитель</param>
        /// <param name="e">аргументы нажатия кнопки</param>
        private void btnSaveHistoryClick(object sender, RoutedEventArgs e)
        {
            string json = JsonConvert.SerializeObject(client.BotMessageLog, Formatting.Indented);
            File.WriteAllText(historyPath, json);

            MessageBox.Show(this, 
                $"История сохранена в {historyPath}",
                this.Title,
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        /// <summary>
        /// Обработка кнопки "Показать клиентскую базу"
        /// </summary>
        /// <param name="sender">отправитель</param>
        /// <param name="e">аругменты нажатия кнопки</param>
        private void btnShowClientBase(object sender, RoutedEventArgs e)
        {
            clientBaseLog.Items.Refresh(); // Сделал рефреш, т.к. не получилось сделать ClientIdComparer для ObservableCollection
            logList.Visibility = Visibility.Hidden;
            clientBaseLog.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Обработка кнопки "Показать историю сообщений"
        /// </summary>
        /// <param name="sender">отправитель</param>
        /// <param name="e">аргументы нажатия кнопки</param>
        private void btnShowChatLog(object sender, RoutedEventArgs e)
        {
            clientBaseLog.Visibility = Visibility.Hidden;
            logList.Visibility = Visibility.Visible;
        }
    }
}

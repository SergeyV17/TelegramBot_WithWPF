namespace Homework_10_VlasenkoS.A
{
    /// <summary>
    /// Класс приодящих сообщений
    /// </summary>
    class MessageLog
    {
        public string Time { get; set; }
        public long Id { get; set; }
        public string Msg { get; set; }
        public string FirstName { get; set; }

        public MessageLog(string Time, long Id, string Msg, string FirstName )
        {
            this.Time = Time;
            this.Id = Id;
            this.Msg = Msg;
            this.FirstName = FirstName;
        }
    }
}

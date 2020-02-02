namespace Homework_10_VlasenkoS.A
{
    /// <summary>
    /// Класс клиентской базы
    /// </summary>
    class ClientBase
    {
        public long Id { get; set; }
        public string FirstName { get; set; }

        public ClientBase(long Id, string FirstName)
        {
            this.Id = Id;
            this.FirstName = FirstName;
        }

        public ClientBase(long Id)
        {
            this.Id = Id;
        }
    }
}

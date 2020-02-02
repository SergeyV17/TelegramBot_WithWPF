using System.Collections.Generic;

namespace Homework_10_VlasenkoS.A
{
    /// <summary>
    /// Класс для сравнения значений Id при поступлении в колекцию ClientBase
    /// </summary>
    class ClientIdComparer : IEqualityComparer<ClientBase>
    {
        public bool Equals(ClientBase x, ClientBase y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(ClientBase obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}

using System.Collections.Generic;

namespace PumpingSystem.Databases
{
    internal interface IQuery<T>
    {
        T Get(string sql, int timeout);
        IEnumerable<T> GetAll(string sql, int timeout);
    }
}

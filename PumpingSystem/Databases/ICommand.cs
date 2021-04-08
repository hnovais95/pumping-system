using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PumpingSystem.Databases
{
    internal interface ICommand
    {
        void Insert(string sql, int timeout);
        void Update(string sql, int timeout);
    }
}

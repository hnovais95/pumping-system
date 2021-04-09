using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PumpingSystem.Common;

namespace PumpingSystem.Server.Repository
{
    public interface IProcessChartDao<T>
    {
        void Insert(T processChart, int timeout);
        IEnumerable<T> GetByPeriod(DateTime startDate, DateTime endDate, int timeout);
    }
}

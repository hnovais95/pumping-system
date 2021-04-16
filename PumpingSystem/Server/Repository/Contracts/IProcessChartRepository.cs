using System;
using System.Collections.Generic;

namespace PumpingSystem.Server.Repository
{
    public interface IProcessChartRepository
    {
        void Insert(object processChart, int timeout);
        List<object> GetByPeriod(DateTime startDate, DateTime endDate, int timeout);
    }
}

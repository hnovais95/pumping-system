using System;
using System.Collections.Generic;

namespace PumpingSystem.Server
{
    internal interface IRepositoryService<T>
    {
        void InsertProcessChart(T processChart);
        IEnumerable<T> GetProcessChartByPeriod(DateTime startDate, DateTime endDate);
    }
}

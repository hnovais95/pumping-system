using System;
using System.Collections.Generic;
using PumpingSystem.Common;
using PumpingSystem.Server.Repository;

namespace PumpingSystem.Server
{
    public class RepositoryService : IRepositoryService<ProcessChart>
    {
        ProcessChartDao _Dao;

        public RepositoryService(ProcessChartDao dao)
        {
            _Dao = dao;
        }

        public void InsertProcessChart(ProcessChart processChart)
        {
            _Dao.Insert(processChart, 5);
        }

        public IEnumerable<ProcessChart> GetProcessChartByPeriod(DateTime startDate, DateTime endDate)
        {
           return _Dao.GetByPeriod(startDate, endDate, 5);
        }
    }
}

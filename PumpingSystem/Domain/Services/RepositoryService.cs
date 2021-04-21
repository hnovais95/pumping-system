using System;
using System.Collections.Generic;
using PumpingSystem.Shared;
using PumpingSystem.Domain.Repository;

namespace PumpingSystem.Domain
{
    public class RepositoryService
    {
        private IProcessChartRepository _ProcessChartRepository;
        private IAuthenticationRepository _AuthenticationRepository;

        public RepositoryService(IProcessChartRepository processChartRepository, IAuthenticationRepository authenticationRepository)
        {
            _ProcessChartRepository = processChartRepository;
            _AuthenticationRepository = authenticationRepository;
        }

        public void InsertProcessChart(ProcessChart processChart)
        {
            _ProcessChartRepository.Insert(processChart, 5);
        }

        public List<ProcessChart> GetProcessChartByPeriod(DateTime startDate, DateTime endDate)
        {
            List<object> gerericObjList = _ProcessChartRepository.GetByPeriod(startDate, endDate, 5);
            List<ProcessChart> processCharts = new List<ProcessChart>();

            if (gerericObjList != null)
            {
                foreach (object obj in gerericObjList)
                {
                    processCharts.Add((ProcessChart)obj);
                }
            }

           return processCharts;
        }

        public void InsertUser(string username, string password, int timeout)
        {
            _AuthenticationRepository.Insert(username, password, 5);
        }

        public bool CheckIfItExistsByUsernameAndPassword(string username, string password, int timeout)
        {
            return _AuthenticationRepository.CheckIfItExistsByUsernameAndPassword(username, password, 5);
        }
    }
}

using System;
using PumpingSystem.Shared;

namespace PumpingSystem.Domain.Repository
{
    [Serializable]
    public sealed class LocalRepository : ILocalRepository
    {
        private static LocalRepository _Instance;

        private const int NUMBER_OF_TANKS = 2;
        public Pump Pump { get; set; }
        public WaterTank[] Tanks { get; set; }
        public ProcessChart ProcessChart { get; set; }

        public LocalRepository()
        {
            this.Pump = new Pump();
            this.Tanks = new WaterTank[NUMBER_OF_TANKS];
            for (int i = 0; i < NUMBER_OF_TANKS; i++)
                this.Tanks[i] = new WaterTank();
            this.ProcessChart = new ProcessChart();
        }

        public static LocalRepository GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new LocalRepository();
            }
            return _Instance;
        }
    }
}

using System;
using PumpingSystem.Common;

namespace PumpingSystem.Server
{
    [Serializable]
    public class RTDB
    {
        private const int NUMBER_OF_TANKS = 2;
        public ProcessChart ProcessChart { get; set; }
        public Pump Pump { get; set; }
        public WaterTank[] Tanks { get; set; }

        public RTDB()
        {
            this.ProcessChart = new ProcessChart();
            this.Pump = new Pump();
            this.Tanks = new WaterTank[NUMBER_OF_TANKS];
            for (int i = 0; i < NUMBER_OF_TANKS; i++)
            {
                this.Tanks[i] = new WaterTank();
            }
        }
    }
}

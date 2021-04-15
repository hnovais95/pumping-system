using System;
using System.Collections.Generic;

namespace PumpingSystem.Common
{
    [Serializable]
    public class ProcessChart
    {
        public List<ProcessChartData> Data { get; }
        public bool Changed { get; set; }

        public ProcessChart()
        {
            this.Data = new List<ProcessChartData>();
            this.Changed = false;
        }

        public void Add(ProcessChartData sample)
        {
            if (this.Data != null)
            {
                this.Data.Add(sample);
                this.Changed = true;
            }
        }
    }

    [Serializable]
    public class ProcessChartData
    {
        public DateTime SampleDate { get; set; }
        public int[] Level { get; set; }
        public int PumpStatus { get; set; }
        public int OperationMode { get; set; }

        public ProcessChartData()
        {
            this.SampleDate = DateTime.Now;
            this.Level = new int[2];
        }

    }
}

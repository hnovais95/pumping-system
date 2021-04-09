using System;
using System.Collections.Generic;

namespace PumpingSystem.Common
{
    [Serializable]
    public class ProcessChart
    {
        public List<DataProcessChart> Data { get; }

        public ProcessChart()
        {
            this.Data = new List<DataProcessChart>();
        }

        public void Add(DataProcessChart sample)
        {
            if (this.Data != null)
            {
                this.Data.Add(sample);
            }
        }
    }

    [Serializable]
    public class DataProcessChart
    {
        public DateTime SampleDate { get; set; }
        public int[] Level { get; set; }
        public int PumpStatus { get; set; }
        public int OperationMode { get; set; }

        public DataProcessChart()
        {
            this.SampleDate = DateTime.Now;
            this.Level = new int[2];
        }

    }
}

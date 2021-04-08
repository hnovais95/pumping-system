using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PumpingSystem.Common
{
    public class ProcessChart
    {
        List<DataProcessChart> Data { get; }

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

    public class DataProcessChart
    {
        public DateTime SampleDate { get; set; }
        public int[] Level { get; set; }
        public int PumpStatus { get; set; }
        public int OperationMode { get; set; }

        public DataProcessChart()
        {
            this.SampleDate = DateTime.Now;
        }

    }
}

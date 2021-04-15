using System.Collections.Generic;
using PumpingSystem.Common;

namespace PumpingSystem.Messages.View
{
    public class MsgChartData
    {
        public List<ProcessChartData> Data { get; set; }

        public MsgChartData()
        {
            this.Data = new List<ProcessChartData>();
        }
    }
}

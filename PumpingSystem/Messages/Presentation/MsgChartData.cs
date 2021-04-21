using System.Collections.Generic;
using PumpingSystem.Shared;

namespace PumpingSystem.Messages.Presentation
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

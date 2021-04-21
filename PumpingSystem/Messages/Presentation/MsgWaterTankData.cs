using System;
using System.Collections.Generic;
using System.Text;

namespace PumpingSystem.Messages.Presentation
{
    public class MsgWaterTankData
    {
        public int Level { get; set; }
        public int MinLevel { get; set; }

        public MsgWaterTankData() : this(0, 0) { }
        public MsgWaterTankData(int level, int minLevel)
        {
            this.Level = level;
            this.MinLevel = minLevel;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace PumpingSystem.Messages.View
{
    public class MsgDataWaterTank
    {
        public int Level { get; set; }
        public int MinLevel { get; set; }

        public MsgDataWaterTank() : this(0, 0) { }
        public MsgDataWaterTank(int level, int minLevel)
        {
            this.Level = level;
            this.MinLevel = minLevel;
        }
    }
}

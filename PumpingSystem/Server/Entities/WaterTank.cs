using System;

namespace PumpingSystem.Server
{
    [Serializable]
    public class WaterTank
    {
        private int _Level;
        private int _MinLevel;
        public bool Changed { get; set; }

        public WaterTank()
        {
            _Level = 0;
            Changed = false;
        }

        public int Level
        {
            get
            {
                return _Level;
            }
            set
            {
                _Level = value;
                Changed = true;
            }
        }

        public int MinLevel
        {
            get
            {
                return _MinLevel;
            }
            set
            {
                _MinLevel = value;
                Changed = true;
            }
        }
    }
}

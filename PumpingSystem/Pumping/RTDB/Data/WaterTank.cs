namespace PumpingSystem.Pumping
{
    public class WaterTank
    {
        private int _Level;
        private int _MinLevel;
        public bool Alterada { get; set; }

        public WaterTank()
        {
            _Level = 0;
            Alterada = false;
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
                Alterada = true;
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
                Alterada = true;
            }
        }
    }
}

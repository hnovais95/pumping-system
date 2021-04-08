using PumpingSystem.Common;

namespace PumpingSystem.Pumping
{
    public class Pumping
    {
        private const int NUMBER_OF_TANKS = 2;
        private EnumOperationMode _OperationMode;
        public Pump Pump { get; }
        public WaterTank[] Tanks { get; }
        public bool Alterada { get; set; }

        public Pumping()
        {
            this.Pump = new Pump();
            this.Tanks = new WaterTank[NUMBER_OF_TANKS];
            for (int i = 0; i < NUMBER_OF_TANKS; i++)
            {
                this.Tanks[i] = new WaterTank();
            }
            _OperationMode = EnumOperationMode.Automatic;
        }

        public EnumOperationMode OperationMode
        {
            get
            {
                return _OperationMode;
            }
            set
            {
                _OperationMode = value;
                this.Alterada = true;
            }
        }

        public void TurnOnPump()
        {
            this.Pump.TurnOnPump();
        }
        public void TurnOffPump()
        {
            this.Pump.TurnOffPump();
        }
    }
}

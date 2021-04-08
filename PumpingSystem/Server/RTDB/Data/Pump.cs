using PumpingSystem.Common;

namespace PumpingSystem.Server
{
    public class Pump
    {
        private EnumPumpStatus _Status;
        private EnumOperationMode _OperationMode;
        public bool Alterada { get; set; } 

        public Pump()
        {
            _Status = EnumPumpStatus.Off;
            _OperationMode = EnumOperationMode.Automatic;
            Alterada = false;
        }

        public EnumPumpStatus Status { get => _Status; }

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
            _Status = EnumPumpStatus.On;
            this.Alterada = true;
        }

        public void TurnOffPump()
        {
            _Status = EnumPumpStatus.Off;
            this.Alterada = true;
        }

    }
}

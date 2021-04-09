using System;
using PumpingSystem.Common;


namespace PumpingSystem.Server
{
    [Serializable]
    public class Pump
    {
        private EnumPumpStatus _Status;
        private EnumOperationMode _OperationMode;
        public bool Changed { get; set; } 

        public Pump()
        {
            _Status = EnumPumpStatus.Off;
            _OperationMode = EnumOperationMode.Automatic;
            Changed = false;
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
                this.Changed = true;
            }
        }

        public void TurnOnPump()
        {
            _Status = EnumPumpStatus.On;
            this.Changed = true;
        }

        public void TurnOffPump()
        {
            _Status = EnumPumpStatus.Off;
            this.Changed = true;
        }

    }
}

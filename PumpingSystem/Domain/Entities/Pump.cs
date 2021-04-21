using System;
using PumpingSystem.Shared;


namespace PumpingSystem.Domain
{
    [Serializable]
    public class Pump
    {
        private PumpStatus _Status;
        private OperationMode _OperationMode;
        public bool Changed { get; set; } 

        public Pump()
        {
            _Status = PumpStatus.Off;
            _OperationMode = OperationMode.Automatic;
            this.Changed = false;
        }

        public PumpStatus Status { get => _Status; }

        public OperationMode OperationMode
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
            _Status = PumpStatus.On;
            this.Changed = true;
        }

        public void TurnOffPump()
        {
            _Status = PumpStatus.Off;
            this.Changed = true;
        }

    }
}

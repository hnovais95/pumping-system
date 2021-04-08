using PumpingSystem.Common;

namespace PumpingSystem.Pumping
{
    public class Pump
    {
        private EnumPumpStatus _Status;
        public bool Alterada { get; set; } 

        public Pump()
        {
            _Status = EnumPumpStatus.Off;
            Alterada = false;
        }

        public EnumPumpStatus Status { get => _Status; }

        public void TurnOnPump()
        {
            _Status = EnumPumpStatus.On;
            Alterada = true;
        }

        public void TurnOffPump()
        {
            _Status = EnumPumpStatus.Off;
            Alterada = true;
        }

    }
}

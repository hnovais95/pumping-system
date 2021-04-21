using PumpingSystem.Shared;

namespace PumpingSystem.Messages.Presentation
{
    public class MsgPumpData
    {
        public PumpStatus StatusPump { get; set; }
        public OperationMode OperationMode { get; set; }

        public MsgPumpData() : this(PumpStatus.Off, OperationMode.Automatic) { }
        public MsgPumpData(PumpStatus statusPump, OperationMode operationMode)
        {
            this.StatusPump = statusPump;
            this.OperationMode = operationMode;
        }
    }
}

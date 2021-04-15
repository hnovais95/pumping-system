using PumpingSystem.Common;

namespace PumpingSystem.Messages.View
{
    public class MsgPumpData
    {
        public EnumPumpStatus StatusPump { get; set; }
        public EnumOperationMode OperationMode { get; set; }

        public MsgPumpData() : this(EnumPumpStatus.Off, EnumOperationMode.Automatic) { }
        public MsgPumpData(EnumPumpStatus statusPump, EnumOperationMode operationMode)
        {
            this.StatusPump = statusPump;
            this.OperationMode = operationMode;
        }
    }
}

using PumpingSystem.Common;

namespace PumpingSystem.Messages.View
{
    public class MsgDataPump
    {
        public EnumPumpStatus StatusPump { get; set; }
        public EnumOperationMode OperationMode { get; set; }

        public MsgDataPump() : this(EnumPumpStatus.Off, EnumOperationMode.Automatic) { }
        public MsgDataPump(EnumPumpStatus statusPump, EnumOperationMode operationMode)
        {
            this.StatusPump = statusPump;
            this.OperationMode = operationMode;
        }
    }
}

using PumpingSystem.Common;

namespace PumpingSystem.Messages.View
{
    public class MsgOperationMode
    {
        public EnumOperationMode OperationMode { get; set; }

        public MsgOperationMode() : this(EnumOperationMode.Automatic) { }
        public MsgOperationMode(EnumOperationMode operationMode)
        {
            this.OperationMode = operationMode;
        }
    }
}

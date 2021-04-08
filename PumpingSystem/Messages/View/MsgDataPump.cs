using PumpingSystem.Common;

namespace PumpingSystem.Messages.View
{
    public class MsgDataPump
    {
        public EnumPumpStatus StatusPump { get; set; }

        public MsgDataPump() : this(EnumPumpStatus.Off) { }
        public MsgDataPump(EnumPumpStatus statusPump)
        {
            this.StatusPump = statusPump;
        }
    }
}

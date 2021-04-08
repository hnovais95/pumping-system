namespace PumpingSystem.Messages.Uart
{
    public class MsgTelegram100 : IMsgUart
    {
        public MsgTelegram100() : this(0) { }

        public MsgTelegram100(int statusPump)
        {
            this.StatusPump = statusPump;
        }

        public int StatusPump { get; set; }

        public int GetID()
        {
            return MsgUartExtension.GetID(this);
        }

        public override string ToString()
        {
            return MsgUartExtension.ToString(this);
        }
    }
}

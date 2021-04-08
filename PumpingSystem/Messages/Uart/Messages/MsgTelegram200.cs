namespace PumpingSystem.Messages.Uart
{
    public class MsgTelegram200 : IMsgUart
    {
        public MsgTelegram200() : this(0) { }

        public MsgTelegram200(int statusPump)
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

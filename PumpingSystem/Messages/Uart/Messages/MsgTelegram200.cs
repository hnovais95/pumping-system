namespace PumpingSystem.Messages.Uart
{
    public class MsgTelegram200 : IMsgUart
    {
        public MsgTelegram200() : this(0, 0) { }

        public MsgTelegram200(int statusPump, int operationMode)
        {
            this.StatusPump = statusPump;
            this.OperationMode = operationMode;
        }

        public int StatusPump { get; set; }
        public int OperationMode { get; set; }

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

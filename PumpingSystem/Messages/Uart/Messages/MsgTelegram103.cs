namespace PumpingSystem.Messages.Uart
{
    public class MsgTelegram103 : IMsgUart
    {
        public MsgTelegram103() : this(0) { }

        public MsgTelegram103(int operationMode)
        {
            this.OperationMode = operationMode;
        }

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

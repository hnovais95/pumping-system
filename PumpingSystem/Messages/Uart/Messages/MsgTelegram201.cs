namespace PumpingSystem.Messages.Uart
{
    public class MsgTelegram201 : IMsgUart
    {
        public MsgTelegram201() : this(0) { }

        public MsgTelegram201(int operationMode)
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

namespace PumpingSystem.Messages.Uart
{
    public class MsgTelegram101 : IMsgUart
    {
        public int MinLevelTank1 { get; set; }
        public int MinLevelTank2 { get; set; }

        public MsgTelegram101() : this(0, 0) { }

        public MsgTelegram101(int minLevelTank1, int minLevelTank2)
        {
            this.MinLevelTank1 = minLevelTank1;
            this.MinLevelTank2 = minLevelTank2;
        }
        
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

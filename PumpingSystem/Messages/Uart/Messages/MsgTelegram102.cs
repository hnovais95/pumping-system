namespace PumpingSystem.Messages.Uart
{
    public class MsgTelegram102 : IMsgUart
    {
        public int LevelTank1 { get; set; }
        public int LevelTank2 { get; set; }

        public MsgTelegram102(): this(0, 0) { }

        public MsgTelegram102(int levelTank1, int levelTank2)
        {
            this.LevelTank1 = levelTank1;
            this.LevelTank2 = levelTank2;
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

namespace PumpingSystem.Messages.Uart
{
    public class MsgTelegram101 : IMsgUart
    {
        public MsgTelegram101(): this(0, 0) { }

        public MsgTelegram101(int levelTank1, int levelTank2)
        {
            this.LevelTank1 = levelTank1;
            this.LevelTank2 = levelTank2;
        }

        public int LevelTank1 { get; set; }
        public int LevelTank2 { get; set; }

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

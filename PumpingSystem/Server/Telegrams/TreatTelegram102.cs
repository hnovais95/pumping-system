using System;
using PumpingSystem.Common;
using PumpingSystem.Messages.Uart;

namespace PumpingSystem.Server
{
    public class TreatTelegram102
    {
        public static void TreatTelegram(IMsgUart msg)
        {
            try
            {
                MsgTelegram102 tel = (MsgTelegram102)msg;
                WaterTank[] waterTanks = Program.RTDB.Tanks;

                waterTanks[(int)EnumWaterTank.Tank1].MinLevel = tel.MinLevelTank1;
                waterTanks[(int)EnumWaterTank.Tank2].MinLevel = tel.MinLevelTank2;
            }
            catch (Exception e)
            {
            }
        }
    }
}
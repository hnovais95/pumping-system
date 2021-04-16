using System;
using PumpingSystem.Common;
using PumpingSystem.Messages.Uart;
using PumpingSystem.Server.Repository;

namespace PumpingSystem.Server
{
    public class TreatTelegram102
    {
        public static void TreatTelegram(IMsgUart msg)
        {
            try
            {
                MsgTelegram102 tel = (MsgTelegram102)msg;

                LocalRepository localRepository = LocalRepository.GetInstance();

                WaterTank[] waterTanks = localRepository.Tanks;
                waterTanks[(int)EnumWaterTank.Tank1].MinLevel = tel.MinLevelTank1;
                waterTanks[(int)EnumWaterTank.Tank2].MinLevel = tel.MinLevelTank2;
            }
            catch (Exception e)
            {
            }
        }
    }
}
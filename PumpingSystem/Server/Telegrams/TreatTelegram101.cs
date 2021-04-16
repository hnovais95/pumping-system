using System;
using PumpingSystem.Common;
using PumpingSystem.Messages.Uart;
using PumpingSystem.Server.Repository;

namespace PumpingSystem.Server
{
    public class TreatTelegram101
    {
        public static void TreatTelegram(IMsgUart msg)
        {
            try
            {
                MsgTelegram101 tel = (MsgTelegram101)msg;

                LocalRepository localRepository = LocalRepository.GetInstance();

                WaterTank[] waterTanks = localRepository.Tanks;
                waterTanks[(int)EnumWaterTank.Tank1].Level = tel.LevelTank1;
                waterTanks[(int)EnumWaterTank.Tank2].Level = tel.LevelTank2;
            }
            catch (Exception e)
            {
            }
        }
    }
}
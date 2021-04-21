using System;
using PumpingSystem.Shared;
using PumpingSystem.Messages.Uart;
using PumpingSystem.Domain.Repository;

namespace PumpingSystem.Domain
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
                waterTanks[(int)WaterTankPosition.Tank1].Level = tel.LevelTank1;
                waterTanks[(int)WaterTankPosition.Tank2].Level = tel.LevelTank2;
            }
            catch (Exception e)
            {
            }
        }
    }
}
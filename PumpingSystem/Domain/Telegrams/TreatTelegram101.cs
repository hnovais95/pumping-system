using System;
using PumpingSystem.Shared;
using PumpingSystem.Messages.Uart;
using PumpingSystem.Domain.Repository;

namespace PumpingSystem.Domain
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
                waterTanks[(int)WaterTankPosition.Tank1].MinLevel = tel.MinLevelTank1;
                waterTanks[(int)WaterTankPosition.Tank2].MinLevel = tel.MinLevelTank2;
            }
            catch (Exception e)
            {
            }
        }
    }
}
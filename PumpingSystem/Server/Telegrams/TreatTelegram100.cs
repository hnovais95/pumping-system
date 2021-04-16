using System;
using PumpingSystem.Messages.Uart;
using PumpingSystem.Common;
using PumpingSystem.Server.Repository;

namespace PumpingSystem.Server
{
    public class TreatTelegram100
    {
        public static void TreatTelegram(IMsgUart msg)
        {
            try
            {
                MsgTelegram100 tel = (MsgTelegram100)msg;

                LocalRepository localRepository = LocalRepository.GetInstance();

                if (tel.StatusPump != (int)localRepository.Pump.Status)
                {
                    if (tel.StatusPump == 1)
                        localRepository.Pump.TurnOnPump();
                    else
                        localRepository.Pump.TurnOffPump();
                }

                if (tel.OperationMode == (int)EnumOperationMode.Automatic)
                {
                    localRepository.Pump.OperationMode = EnumOperationMode.Automatic;
                }
                else
                {
                    localRepository.Pump.OperationMode = EnumOperationMode.Manual;
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}
using System;
using PumpingSystem.Messages.Uart;
using PumpingSystem.Shared;
using PumpingSystem.Domain.Repository;

namespace PumpingSystem.Domain
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

                if (tel.OperationMode == (int)OperationMode.Automatic)
                {
                    localRepository.Pump.OperationMode = OperationMode.Automatic;
                }
                else
                {
                    localRepository.Pump.OperationMode = OperationMode.Manual;
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}
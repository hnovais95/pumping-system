using System;
using PumpingSystem.Messages.Uart;
using PumpingSystem.Pumping;

namespace PumpingSystem
{
    public class TreatTelegram100
    {
        public static void TreatTelegram(IMsgUart msg)
        {
            try
            {
                MsgTelegram100 tel = (MsgTelegram100)msg;
                Pump pump = Program.RTDB.Pumping.Pump;

                if (tel.StatusPump != (int)pump.Status)
                {
                    if (tel.StatusPump == 1) 
                        pump.TurnOnPump();
                    else 
                        pump.TurnOffPump();
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}
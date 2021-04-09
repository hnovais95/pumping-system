﻿using System;
using PumpingSystem.Messages.Uart;

namespace PumpingSystem.Server
{
    public class TreatTelegram100
    {
        public static void TreatTelegram(IMsgUart msg)
        {
            try
            {
                MsgTelegram100 tel = (MsgTelegram100)msg;
                Pump pump = Program.RTDB.Pump;

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
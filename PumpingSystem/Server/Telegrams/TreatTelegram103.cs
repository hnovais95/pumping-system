using System;
using PumpingSystem.Common;
using PumpingSystem.Messages.Uart;

namespace PumpingSystem.Server
{
    public class TreatTelegram103
    {
        public static void TreatTelegram(IMsgUart msg)
        {
            try
            {
                MsgTelegram103 tel = (MsgTelegram103)msg;

                if (tel.OperationMode == (int)EnumOperationMode.Automatic)
                {
                    Program.RTDB.Pump.OperationMode = EnumOperationMode.Automatic;
                }
                else
                {
                    Program.RTDB.Pump.OperationMode = EnumOperationMode.Manual;
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}
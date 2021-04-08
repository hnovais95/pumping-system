using System;
using PumpingSystem.Common;
using PumpingSystem.Messages.Uart;
using PumpingSystem.Pumping;

namespace PumpingSystem
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
                    Program.RTDB.Pumping.OperationMode = EnumOperationMode.Automatic;
                }
                else
                {
                    Program.RTDB.Pumping.OperationMode = EnumOperationMode.Manual;
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}
using System;

namespace PumpingSystem.Messages.Uart
{
    public class MsgUartTypes
    {
        public static Type GetTelegramTypeByID(int telegramID)
        {
            switch (telegramID)
            {
                case 100:
                    return typeof(MsgTelegram100);
                case 101:
                    return typeof(MsgTelegram101);
                case 102:
                    return typeof(MsgTelegram102);
                case 103:
                    return typeof(MsgTelegram103);
                default:
                    throw new Exception(String.Format("Não conseguiu encontrar tipo da classe para telegrama {0}", telegramID));
            }
        }
    }
}
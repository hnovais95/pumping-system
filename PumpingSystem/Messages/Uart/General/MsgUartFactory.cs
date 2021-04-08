using System;

namespace PumpingSystem.Messages.Uart
{
    public class MsgUartFactory
    {
        public static IMsgUart CreateMsg(int telegramID)
        {
            try
            {
                return (IMsgUart)Activator.CreateInstance(MsgUartTypes.GetTelegramTypeByID(telegramID));
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("Na construção do telegrama {0} pelo ID", telegramID), e);
            }
        }
    }
}
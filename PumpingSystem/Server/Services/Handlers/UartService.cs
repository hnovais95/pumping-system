using PumpingSystem.Messages.Uart;

namespace PumpingSystem.Server
{
    public class UartService
    {
        private MsgUartHandlers m_Handlers;

        public UartService()
        {
            m_Handlers = new MsgUartHandlers();
            m_Handlers.AddHandler(100, new MsgUartHandlers.MsgUartHandler(TreatTelegram100.TreatTelegram));
            m_Handlers.AddHandler(101, new MsgUartHandlers.MsgUartHandler(TreatTelegram101.TreatTelegram));
            m_Handlers.AddHandler(102, new MsgUartHandlers.MsgUartHandler(TreatTelegram102.TreatTelegram));
            m_Handlers.AddHandler(103, new MsgUartHandlers.MsgUartHandler(TreatTelegram103.TreatTelegram));
        }

        public void TreatMessage(IMsgUart msg)
        {
            m_Handlers.TreatMessage(msg);
        }

        public void SendMessage(IMsgUart msg)
        {
            Program.UartDriver.SendDataHandler(msg);
        }
    }
}

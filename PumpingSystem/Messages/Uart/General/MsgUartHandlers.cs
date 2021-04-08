// ------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace PumpingSystem.Messages.Uart
{
    public class MsgUartHandlers
    {
        public delegate void MsgUartHandler(IMsgUart msg);

        private Dictionary<int, MsgUartHandler> m_Handlers;

        public MsgUartHandlers()
        {
            m_Handlers = new Dictionary<int, MsgUartHandler>();
        }

        public void AddHandler(int telegramID, MsgUartHandler handler)
        {
            try { Type t = MsgUartTypes.GetTelegramTypeByID(telegramID); }
            catch { throw new Exception(String.Format("Telegrama {0} não existe no PIC. Impossível adicionar tratamento", telegramID)); }
            m_Handlers.Add(telegramID, handler);
        }

        public void TreatMessage(IMsgUart msg)
        {
            if (m_Handlers.ContainsKey(msg.GetID()))
                m_Handlers[msg.GetID()](msg);
            else
                throw new Exception(String.Format("Tratamento do telegrama {0} do PIC não foi inicializado", msg.GetID()));
        }
    }
}
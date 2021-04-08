using System;
using System.Text;
using System.Reflection;

namespace PumpingSystem.Messages.Uart
{
    internal static class MsgUartExtension
    {
        internal static int GetID(this IMsgUart self)
        {
            string className = self.GetType().Name;
            int firstDigitIndex = className.IndexOfAny("0123456789".ToCharArray());
            if (firstDigitIndex < 0)
            {
                throw new IndexOutOfRangeException("Nome da classe com tamanho inválido.");
            }
            return int.Parse(className.Substring(firstDigitIndex));
        }

        internal static string ToString(this IMsgUart self)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("[{0}] ->", self.GetID());

            PropertyInfo[] propsInfo = self.GetType().GetProperties();
            if ((propsInfo != null) && (propsInfo.Length > 0))
            {
                foreach (PropertyInfo prop in propsInfo)
                {
                    sb.AppendFormat(" {0}:{1}", prop.Name, prop.GetValue(self, null));
                }
            }

            return sb.ToString();
        }
    }
}

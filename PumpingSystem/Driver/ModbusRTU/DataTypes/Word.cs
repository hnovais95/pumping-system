using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace IndustrialNetworks.ModbusRTU.DataTypes
{
    public class Word
    {
        //Convert a two bytes to a Word:
        public static ushort ConvertByteArrayToWord(byte HiVal, byte LoVal)
        {
            return (ushort)(HiVal * 256 + LoVal);
        }

        //Convert a byte array to a Word array:
        public static ushort[] ConvertByteArrayToWordArray(byte[] bytes)
        {
            UInt16[] values = new UInt16[bytes.Length / 2];
            int counter = 0;
            for (int cnt = 0; cnt < bytes.Length / 2; cnt++)
                values[cnt] = ConvertByteArrayToWord(bytes[counter++], bytes[counter++]);
            return values;
        }

        // Convert a Word to a byte array
        public static byte[] ConvertWordToByteArray(UInt16 value)
        {
            byte[] array = BitConverter.GetBytes(value);
            Array.Reverse(array);
            return array;
        }

        // Convert a Word array to a byte array
        public static byte[] ConvertWordArrayToByteArray(UInt16[] value)
        {
            ByteArray arr = new ByteArray();
            foreach (UInt16 val in value)
                arr.Add(ConvertWordToByteArray(val));
            return arr.array;
        }
    }
}

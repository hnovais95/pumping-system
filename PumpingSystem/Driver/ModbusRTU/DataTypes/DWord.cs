using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace IndustrialNetworks.ModbusRTU.DataTypes
{
    public class DWord
    {
        // Convert a Byte Array To a DWord
        public static UInt32 ConvertByteArrayToDWord(byte[] bytes)
        {
            Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }

        // Convert a Byte Array To a DWord Array
        public static UInt32[] ConvertByteArrayToDWordArray(byte[] bytes)
        {
            UInt32[] values = new UInt32[bytes.Length / 4];
            int counter = 0;
            for (int cnt = 0; cnt < bytes.Length / 4; cnt++)
                values[cnt] = ConvertByteArrayToDWord(new byte[] { bytes[counter++], bytes[counter++], bytes[counter++], bytes[counter++] });
            return values;
        }

        // Convert a DWord To a Byte Array
        public static byte[] ConvertDWordToByteArray(UInt32 value)
        {
            byte[] byteArray = BitConverter.GetBytes(value);
            Array.Reverse(byteArray);
            return byteArray;
        }

        // Convert a DWord Array To a Byte Array 
        public static byte[] ConvertDWordArrayToByteArray(UInt32[] value)
        {
            ByteArray arr = new ByteArray();
            foreach (UInt32 val in value)
                arr.Add(ConvertDWordToByteArray(val));
            return arr.array;
        }
    }
}

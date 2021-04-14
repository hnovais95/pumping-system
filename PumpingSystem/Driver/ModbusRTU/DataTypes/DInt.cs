using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace IndustrialNetworks.ModbusRTU.DataTypes
{
    public class DInt
    {
        // Convert four bytes to a DInt
        public static int ConvertByteArrayToDInt(byte[] bytes)
        {
            Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);   
        }

        // Convert a Byte Array To a DInt Array
        public static int[] ConvertByteArrayToDIntArray(byte[] bytes)
        {
            int[] values = new int[bytes.Length / 4];

            int counter = 0;
            for (int cnt = 0; cnt < bytes.Length / 4; cnt++)
                values[cnt] = ConvertByteArrayToDInt(new byte[] { bytes[counter++], bytes[counter++], bytes[counter++], bytes[counter++] });
            return values;
        }

        // Convert a DInt To a Byte Array
        public static byte[] ConvertDIntToByteArray(int value)
        {
            byte[] array = BitConverter.GetBytes(value);
            Array.Reverse(array);
            return array;
        }

        // Convert a Byte Array To a DInt Array
        public static byte[] ConvertDIntToByteArray(int[] value)
        {
            ByteArray arr = new ByteArray();
            foreach (int val in value)
                arr.Add(ConvertDIntToByteArray(val));
            return arr.array;
        }
    }
}

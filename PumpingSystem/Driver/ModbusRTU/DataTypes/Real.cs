using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace IndustrialNetworks.ModbusRTU.DataTypes
{
    public class Real
    {
        // Convert a Byte Array To a Real
        public static float ConvertByteArrayToRealInverse(byte[] bytes)
        {
            Array.Reverse(bytes);
            return BitConverter.ToSingle(bytes, 0);
        }

        // Convert a Byte Array To a Real
        public static float ConvertByteArrayToReal(byte[] bytes)
        {
            if (bytes.Length != 4) throw new FormatException("Size of byte array > 4)");
            Array.Reverse(bytes);
            int size = bytes.Length / 2;
            for (int i = 0; i < size; i++)
            {
                bytes[i] += bytes[i + size];
                bytes[i + size] = (byte)(bytes[i] - bytes[i + size]);
                bytes[i] = (byte)(bytes[i] - bytes[i + size]);
            }
            return BitConverter.ToSingle(bytes, 0);
        }

        // Convert a Real To a Byte Array
        public static byte[] ConvertRealToByteArray(float value)
        {
            byte[] array = BitConverter.GetBytes(value);
            Array.Reverse(array);
            return array;
        }

        // Convert a Real arary To a Byte Array
        public static byte[] ToByteArray(float[] value)
        {
            ByteArray arr = new ByteArray();
            foreach (float val in value)
                arr.Add(ConvertRealToByteArray(val));
            return arr.array;
        }

        // Convert a Byte Array To a Float Array Inverse
        public static float[] ConvertByteArrayToFloatArrayInverse(byte[] bytes)
        {
            float[] values = new float[bytes.Length / 4];

            int counter = 0;
            for (int cnt = 0; cnt < bytes.Length / 4; cnt++)
                values[cnt] = ConvertByteArrayToRealInverse(new byte[] { bytes[counter++], bytes[counter++], bytes[counter++], bytes[counter++] });
            return values;
        }

        // Convert a Byte Array To a Float Array 
        public static float[] ConvertByteArrayToFloatArray(byte[] bytes)
        {
            int size = 4;
            int idx = 0;
            float[] result = new float[bytes.Length / size];
            do
            {
                byte[] data = new byte[size];
                Array.Copy(bytes, idx, data, 0, data.Length);
                result[idx / size] = ConvertByteArrayToReal(data);
                idx += size;
            } while (idx < bytes.Length);
            return result;
        }
    }
}

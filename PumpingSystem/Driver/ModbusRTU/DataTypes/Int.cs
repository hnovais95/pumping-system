using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace IndustrialNetworks.ModbusRTU.DataTypes
{
public class Int
{

    // Convert a two bytes to an int:
    public static short ConvertByteArrayToInt(byte HiVal, byte LoVal)
    {
        return (short)(HiVal * 256 + LoVal);
    }

    // Convert a byte array to an int array:
    public static short[] ConvertByteArrayToIntArray(byte[] bytes)
    {
        short[] values = new short[bytes.Length / 2];
        int counter = 0;
        for (int cnt = 0; cnt < bytes.Length / 2; cnt++)
            values[cnt] = ConvertByteArrayToInt(bytes[counter++], bytes[counter++]);
        return values;
    }

    // Convert an Int to a byte array
    public static byte[] ConvertIntToByteArray(short value)
    {
        byte[] byteArray = BitConverter.GetBytes(value);
        Array.Reverse(byteArray);
        return byteArray;
    }

    // Convert an Int array to a byte array
    public static byte[] ConvertIntArrayToByteArray(short[] value)
    {
        ByteArray arr = new ByteArray();
        foreach (short val in value)
            arr.Add(ConvertIntToByteArray(val));
        return arr.array;
    }
}
}

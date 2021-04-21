using System;
using System.IO.Compression;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace PumpingSystem.Shared.Utilities
{
    public class Utilities
    {
        public static SerializationBinder CustomBinder { get; set; }

        /// <summary>
        /// Serialize object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Byte array</returns>
        public static byte[] Serialize(object obj)
        {
            if (obj == null) return new byte[0];
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        /// <summary>
        /// Deserializes object
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        /// <returns>Deserialized object</returns>
        public static object Deserialize(Stream buffer, long index)
        {
            if (buffer == null || index > buffer.Length - 1)
                return null;
            buffer.Position = index;
            var bf = new BinaryFormatter();
            if (CustomBinder != null)
                bf.Binder = CustomBinder;
            return bf.Deserialize(buffer);
        }

        /// <summary>
        /// Deserializes object
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns>Deserialized object</returns>
        public static object ObjDeserialize(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return null;
            return Deserialize(new MemoryStream(bytes, 0, bytes.Length), 0);
        }

        /// <summary>
        /// Compact byte array
        /// </summary>
        /// <param name="dados"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] dados)
        {
            if ((dados == null) || (dados.Length == 0)) return null;
            MemoryStream mso = new MemoryStream();
            using (MemoryStream msi = new MemoryStream(dados))
            {
                using (mso = new MemoryStream())
                {
                    using (GZipStream gzip = new GZipStream(mso, CompressionMode.Compress))
                    {
                        msi.CopyTo(gzip);
                    }
                }
            }
            return mso.ToArray();
        }

        /// <summary>
        /// Unpacks byte array
        /// </summary>
        /// <param name="dados"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] dados)
        {
            if ((dados == null) || (dados.Length == 0)) return null;
            MemoryStream mso = new MemoryStream();
            using (MemoryStream msi = new MemoryStream(dados))
            {
                using (mso = new MemoryStream())
                {
                    using (GZipStream gzip = new GZipStream(msi, CompressionMode.Decompress))
                    {
                        gzip.CopyTo(mso);
                    }
                }
            }
            return mso.ToArray();
        }

        /// <summary>
        /// Get start of the minute
        /// </summary>
        /// <param name="dth"></param>
        /// <returns></returns>
        public static DateTime GetStartMinute(DateTime dth)
        {
            return new DateTime(dth.Year, dth.Month, dth.Day, dth.Hour, dth.Minute, 0, 0);
        }

        /// <summary>
        /// Get end of the minute
        /// </summary>
        /// <param name="dth"></param>
        /// <returns></returns>
        public static DateTime GetEndMinute(DateTime dth)
        {
            return new DateTime(dth.Year, dth.Month, dth.Day, dth.Hour, dth.Minute, 59, 999);
        }
    }
}

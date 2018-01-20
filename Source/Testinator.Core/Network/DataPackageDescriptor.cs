using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Testinator.Core
{
    /// <summary>
    /// Provides methods do descript a <see cref="DataPackage"/>
    /// </summary>
    public static class DataPackageDescriptor
    {
        /// <summary>
        /// Attempts to get a <see cref="T"/> from binary
        /// </summary>
        /// <param name="Bytes">Bytes to be converted</param>
        /// <param name="output">Formated T object</param>
        /// <returns>True is operation was successful, otherwise false</returns>
        public static bool TryConvertToObj<T>(byte[] Bytes, out T output)
            where T : class
        {
            try
            {
                var ms = new MemoryStream();
                var bf = new BinaryFormatter();
                ms.Write(Bytes, 0, Bytes.Length);
                ms.Seek(0, SeekOrigin.Begin);
                output = (T)bf.Deserialize(ms);
            }
            catch
            {
                output = null;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the binary of this class
        /// Usefull for sending operation
        /// </summary>
        /// <param name="Bytes">Output binary</param>
        /// <param name="input">Object to be converted</param>
        /// <returns>True is operation was successful, otherwise false</returns>
        public static bool TryConvertToBin(out byte[] Bytes, object input)
        {
            try
            {
                var bf = new BinaryFormatter();
                using (var ms = new MemoryStream())
                {
                    bf.Serialize(ms, input);
                    Bytes = ms.ToArray();
                }
            }
            catch(Exception ex)
            {
                Bytes = new byte[0];
                return false;
            }
            return true;
        }
    }
}
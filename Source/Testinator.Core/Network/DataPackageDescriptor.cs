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
        /// Attepmts to fromat a <see cref="DataPackage"/> from binary
        /// </summary>
        /// <param name="Bytes">Bytes to be converted</param>
        /// <param name="output">Formated data package</param>
        /// <returns>True is operation was sucessfull, otherwise false</returns>
        public static bool TryConvertToObj(byte[] Bytes, out DataPackage output)
        {
            try
            {
                var ms = new MemoryStream();
                var bf = new BinaryFormatter();
                ms.Write(Bytes, 0, Bytes.Length);
                ms.Seek(0, SeekOrigin.Begin);
                output = (DataPackage)bf.Deserialize(ms);
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
        /// <returns>True is operation was sucessfull, otherwise false</returns>
        public static bool TryConvertToBin(out byte[] Bytes, DataPackage input)
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
            catch
            {
                Bytes = new byte[0];
                return false;
            }
            return true;
        }
    }
}
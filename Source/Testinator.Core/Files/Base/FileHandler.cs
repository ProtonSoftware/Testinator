namespace Testinator.Core
{
    /// <summary>
    /// Handles working with file writers/readers
    /// </summary>
    public static class FileHandler
    {
        #region Static Properties

        /// <summary>
        /// The writer for xml files
        /// </summary>
        public static XmlWriter XmlWriter { get; private set; }

        /// <summary>
        /// The writer for binary files
        /// </summary>
        public static BinaryWriter BinaryWriter { get; private set; }

        /// <summary>
        /// The reader for xml files
        /// </summary>
        public static XmlReader XmlReader { get; private set; }

        /// <summary>
        /// The reader for binary files
        /// </summary>
        public static BinaryReader BinaryReader { get; private set; }

        #endregion

        #region Public Helpers

        /// <summary>
        /// Setups the <see cref="XmlWriter"/> to provided type
        /// </summary>
        public static void SetupXmlWriter(SaveableObjects type)
        {
            XmlWriter = new XmlWriter(type);
        }

        /// <summary>
        /// Setups the <see cref="BinaryWriter"/> to provided type
        /// </summary>
        public static void SetupBinaryWriter(SaveableObjects type)
        {
            BinaryWriter = new BinaryWriter(type);
        }

        /// <summary>
        /// Setups the <see cref="XmlReader"/> to provided type
        /// </summary>
        public static void SetupXmlReader(SaveableObjects type)
        {
            XmlReader = new XmlReader(type);
        }

        /// <summary>
        /// Setups the <see cref="BinaryReader"/> to provided type
        /// </summary>
        public static void SetupBinaryReader(SaveableObjects type)
        {
            BinaryReader = new BinaryReader(type);
        }

        #endregion
    }
}

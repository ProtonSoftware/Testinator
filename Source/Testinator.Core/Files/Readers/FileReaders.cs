namespace Testinator.Core
{
    /// <summary>
    /// Contains all available file readers
    /// </summary>
    public static class FileReaders
    {
        public static XmlReader XmlReader = new XmlReader();

        public static BinaryReader BinReader = new BinaryReader();
    }
}

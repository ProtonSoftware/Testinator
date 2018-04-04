namespace Testinator.Core
{
    public interface IFileManager
    {
        void WriteToFile(string FileName, string FilePath, byte[] Content);

        void DeleteFile(string FileName, string FilePath);

        byte[] ReadFile(string FileName, string FilePath);
    }
}

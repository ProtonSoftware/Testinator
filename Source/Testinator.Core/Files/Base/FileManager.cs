using System;
using System.IO;

namespace Testinator.Core
{
    /// <summary>
    /// Manages file writing, reading and deleting
    /// </summary>
    public abstract class FileManager : IFileManager
    {
        #region Public Proprties

        public abstract string RootDirectoryPath { get; set; }

        #endregion

        #region Public Methods

        public void DeleteFile(string FileName, string FilePath)
        {
            if (string.IsNullOrEmpty(FileName) || string.IsNullOrEmpty(FilePath))
                throw new NullReferenceException("Value cannot be null");

            if (!Directory.Exists(FilePath))
                throw new Exception("Directory does not exist");

            if (!File.Exists(FilePath + "\\" + FileName))
                throw new Exception("File does not exist");

            File.Delete(FilePath + "\\" + FileName);

        }

        public byte[] ReadFile(string FileName, string FilePath)
        {
            if (string.IsNullOrEmpty(FileName) || string.IsNullOrEmpty(FilePath))
                throw new NullReferenceException("Value cannot be null");

            if (!Directory.Exists(FilePath))
                throw new Exception("Directory does not exist");

            if (!File.Exists(FilePath + "\\" + FileName))
                throw new Exception("File does not exist");

            File.ReadAllBytes

        }

        public void WriteToFile(string FileName, string FilePath, byte[] Content)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}

using System.Collections.Generic;
using System.IO;

namespace Testinator.Core
{
    /// <summary>
    /// The file writer which writes in binary code
    /// </summary>
    public class BinaryWriter : FileWriterBase
    {
        /// <summary>
        /// Writes a test to file
        /// </summary>
        /// <param name="test">The test to be written to file</param>
        public override void WriteToFile(Test test)
        {
            // Make sure we have test to write
            if (test == null)
                return;

            var filename = string.Empty;
            if (BinaryReader.Tests.ContainsKey(test.ID))
                filename = BinaryReader.Tests[test.ID];
            else
                filename = CreateFileName();
            
            if (DataPackageDescriptor.TryConvertToBin(out var dataBin, new DataPackage(PackageType.TestForm, test)))
            {
                using (var writer = new System.IO.BinaryWriter(File.Open(Settings.Path + "Tests\\" + filename + ".dat", FileMode.Create)))
                {
                     writer.Write(dataBin);
                }
            }

            // Reload the test list
            FileReaders.BinReader.ReadAllTests();
        }

        private string CreateFileName()
        {
            var Name = "test";
            var Files = new List<string>();
            var FileIndexes = new List<int>();
            
            try
            {
                Files = new List<string>(Directory.GetFiles(Settings.Path + "Tests\\"));
            }
            catch
            {
                return null;
            }
            foreach(var file in Files)
            {
                if (Path.GetExtension(file) == ".dat")
                { 
                    var fileName = Path.GetFileNameWithoutExtension(file);

                    // If the file name is in correct format
                    if (fileName.StartsWith(Name))
                    {
                        var FileNumberStr = fileName.Substring(4);
                        if (int.TryParse(FileNumberStr, out var fileNumber))
                        {
                            FileIndexes.Add(fileNumber);
                        }
                    }
                }
            }

            FileIndexes.Sort();
            var index = 1;
            while (FileIndexes.Contains(index))
                index++;

            return Name + index.ToString();
        }

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BinaryWriter()
        {
            Settings = new WriterSettings();
        }

        #endregion
    }
}

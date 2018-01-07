using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Testinator.Core
{
    /// <summary>
    /// The file writer which writes in binary code
    /// </summary>
    public class BinaryWriter : FileWriterBase
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BinaryWriter()
        {
            Settings = new WriterSettings();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Deletes a test
        /// </summary>
        /// <param name="test">Test to be deleted</param>
        public override void DeleteFile(Test test)
        {
            if (test == null)
                return;

            var filename = string.Empty;
            if (BinaryReader.Tests.ContainsKey(test.ID))
                filename = BinaryReader.Tests[test.ID];
            else
                return;

            try
            {
                File.Delete(Settings.Path + "Tests\\" + filename + ".dat");
            }
            catch
            {
                // TODO: file protected etc.
            }

        }

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

        public override void WriteToFile(ClientTestResults results)
        {
            if (results == null)
                return;

            var filenameOriginal = CreateClientResultsFileName(results);
            var filenameFinal = filenameOriginal;
            var suffix = 1;
            while (File.Exists(Settings.Path + "Results\\" + filenameFinal + ".dat"))
            {
                filenameFinal = filenameOriginal + "(" + suffix + ")";
                suffix++;
            }

            if (DataPackageDescriptor.TryConvertToBin(out var dataBin, results))
            {
                using (var writer = new System.IO.BinaryWriter(File.Open(Settings.Path + "Results\\" + filenameFinal + ".dat", FileMode.Create)))
                {
                    writer.Write(dataBin);
                }
            }
        }

        public override void WriteToFile(TestResults results)
        {
            if (results == null)
                return;

            var filenameOriginal = CreateResultsFileName(results);
            var filenameFinal = filenameOriginal;
            var suffix = 1;
            while(File.Exists(Settings.Path + "Results\\" + filenameFinal + ".dat"))
            {
                filenameFinal = filenameOriginal + "(" + suffix + ")";
                suffix++;
            }

            if (DataPackageDescriptor.TryConvertToBin(out var dataBin, results))
            {
                using (var writer = new System.IO.BinaryWriter(File.Open(Settings.Path + "Results\\" + filenameFinal + ".dat", FileMode.Create)))
                {
                    writer.Write(dataBin);
                }
            }
        }
        
        #endregion

        #region Private Helpers

        private string CreateClientResultsFileName(ClientTestResults results)
        {
            var hours = DateTime.Now.ToShortTimeString().Replace(':', '-');
            var date = DateTime.Now.ToShortDateString().Replace('/', '-');
            return results.ClientModel.ClientName + "-" + results.ClientModel.ClientSurname + "-" + date + "-" + hours;
        }

        private string CreateResultsFileName(TestResults result)
        {
            var hours = result.Date.ToShortTimeString().Replace(':', '-');
            var date = result.Date.ToShortDateString().Replace('/', '-');
            return "Result" + "-" + date + "-" + hours;
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

        #endregion
    }
}

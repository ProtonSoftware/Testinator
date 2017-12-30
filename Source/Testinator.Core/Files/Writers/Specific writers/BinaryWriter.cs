using System.Collections.Generic;
using System.IO;

namespace Testinator.Core
{
    public class BinaryWriter : FileWriterBase
    {
        /// <summary>
        /// Writes a test to file
        /// </summary>
        /// <param name="test">The test to be written to file</param>
        public override void WriteToFile(Test test)
        {
            if (test == null)
                return;
            
            if (DataPackageDescriptor.TryConvertToBin(out var dataBin, new DataPackage(PackageType.TestForm, test)))
            {
                using (var writer = new System.IO.BinaryWriter(File.Open(Settings.Path + "Tests\\" + CreateFileName() + ".dat", FileMode.Create)))
                {
                     writer.Write(dataBin);
                }
            }
        }

        private string CreateFileName()
        {
            var result = "test";
            var Files = new List<string>();
            var FileNames = new List<string>();
            
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
                    FileNames.Add(fileName);
                }

            }
            var index = 1;

            foreach (var file in FileNames)
            {
                if (file != result + index.ToString())
                    break;
                index++;
            }

            return result + index.ToString();
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

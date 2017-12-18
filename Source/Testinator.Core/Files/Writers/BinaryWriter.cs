using System.Collections.Generic;
using System.IO;

namespace Testinator.Core
{
    public class BinaryWriter : WriterBase
    {

        /// <summary>
        /// Writs a test to file
        /// </summary>
        /// <param name="test">The test to be written to file</param>
        public void WriteTestToFile(Test test)
        {
            if (test == null)
                return;

            Directory.CreateDirectory(Settings.Path + "Tests\\");

            if (DataPackageDescriptor.TryConvertToBin(out byte[] dataBin, new DataPackage(PackageType.TestForm, test)))
            {
                using (System.IO.BinaryWriter writer = new System.IO.BinaryWriter(File.Open(Settings.Path + "Tests\\" + CreateFileName() + ".dat", FileMode.Create)))
                {
                     writer.Write(dataBin);
                }
            }
        }

        private string CreateFileName()
        {
            var restult = "test";
            List<string> Files;
            List<string> FileNames = new List<string>();
            
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
            int Index = 1;

            foreach (var file in FileNames)
            {
                if (file != restult + Index.ToString())
                    break;
                Index++;
            }

            return restult + Index.ToString();
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

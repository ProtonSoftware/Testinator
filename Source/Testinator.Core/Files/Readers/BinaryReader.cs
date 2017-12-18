using System;
using System.Collections.Generic;
using System.IO;

namespace Testinator.Core
{
    public class BinaryReader : ReaderBase
    {

        /// <summary>
        /// Gets all tests from the directory
        /// </summary>
        /// <returns></returns>
        public List<Test> ReadAllTests()
        {
            List<string> Files;
            try
            {
                Files = new List<string>(Directory.GetFiles(Settings.Path + "Tests\\"));
            }
            catch
            {
                return null;
            }

            var tests = new List<Test>();

            foreach (var file in Files)
            {
                try
                {
                    if (Path.GetExtension(file) != ".dat")
                        continue;

                    var filecontent = GetTestFromBin(file);
                    if (filecontent != null)
                        tests.Add(filecontent);
                }
                catch
                {
                    // No error handlig for now
                }
            }
            return tests;

        }

        private Test GetTestFromBin(string file)
        {
            var data = File.ReadAllBytes(file);
            if (DataPackageDescriptor.TryConvertToObj(data, out DataPackage dataP))
            {
                return dataP.Content as Test;
            }
            return null;
        }

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BinaryReader()
        {
            Settings = new ReaderSettings();
        }

        #endregion
    }
}

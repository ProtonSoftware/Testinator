using System;
using System.Collections.Generic;
using System.IO;

namespace Testinator.Core
{
    public class BinaryReader : FileReaderBase
    {
        #region Public Methods

        /// <summary>
        /// Gets all tests from the directory
        /// </summary>
        /// <returns>List of all available tests</returns>
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

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets a test object from file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private Test GetTestFromBin(string file)
        {
            var data = File.ReadAllBytes(file);
            if (DataPackageDescriptor.TryConvertToObj(data, out DataPackage dataP))
            {
                return dataP.Content as Test;
            }
            return null;
        }

        #endregion
        
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

using System.Collections.Generic;
using System.IO;

namespace Testinator.Core
{
    /// <summary>
    /// The binary files reader
    /// </summary>
    public class BinaryReader : FileManagerBase
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BinaryReader(SaveableObjects objectType)
        {
            ObjectType = objectType;
        }

        #endregion

        #region Public Methods

        public static Dictionary<int, string> Tests { get; set; } = new Dictionary<int, string>();

        public static Dictionary<TestResults, string> Results { get; set; } = new Dictionary<TestResults, string>();
        
        /// <summary>
        /// Gets all tests from the directory
        /// </summary>
        /// <returns>List of all available tests</returns>
        public List<Test> ReadAllTests()
        {
            Tests = new Dictionary<int, string>();

            var tests = new List<Test>();
            var indexer = 0;

            foreach (var file in GetFileNames())
            {
                try
                {
                    if (Path.GetExtension(file) != ".dat")
                        continue;

                    var filecontent = GetObjectFromFile<Test>(file, true);
                    if (filecontent != null)
                    {
                        filecontent.ID = indexer;
                        Tests.Add(indexer, Path.GetFileNameWithoutExtension(file));
                        tests.Add(filecontent);

                        indexer++;
                    }
                }
                catch
                {
                    // TODO: Error handling
                }
            }
            return tests;

        }

        /// <summary>
        /// Gets all of the results from the directory
        /// </summary>
        /// <returns>List of test results found on the machine</returns>
        public List<TestResults> ReadAllResults()
        {
            Results = new Dictionary<TestResults, string>();
            Tests = new Dictionary<int, string>();

            var results = new List<TestResults>();
            foreach (var file in GetFileNames())
            {
                try
                {
                    if (Path.GetExtension(file) != ".dat")
                        continue;

                    var filecontent = GetObjectFromFile<TestResults>(file);
                    if (filecontent != null)
                    {
                        results.Add(filecontent);
                        Results.Add(filecontent, Path.GetFileNameWithoutExtension(file));
                    }
                }
                catch
                {
                    // TODO: Error handling
                }
            }
            return results;
        }

        #endregion
    }
}

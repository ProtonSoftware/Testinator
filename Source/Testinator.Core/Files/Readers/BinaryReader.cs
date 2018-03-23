using System.Collections.Generic;
using System.IO;

namespace Testinator.Core
{
    /// <summary>
    /// The binary files reader
    /// </summary>
    public class BinaryReader : FileManagerBase
    {
        // TODO: Fix that, it shouldn't be there, not like this
        #region Public Properties

        public static Dictionary<int, string> TestIDs { get; set; } = new Dictionary<int, string>();

        public static Dictionary<ServerTestResultsBase, string> Results { get; set; } = new Dictionary<ServerTestResultsBase, string>();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BinaryReader(SaveableObjects objectType)
        {
            ObjectType = objectType;
        }

        #endregion
        
        #region File Reading

        /// <summary>
        /// Reads every files in the directory and creates <see cref="T"/> objects from them
        /// </summary>
        /// <typeparam name="T">The type of an object to get from files</typeparam>
        /// <returns>List of T objects</returns>
        public override List<T> ReadFile<T>()
        {
            // Reset the dictionaries
            Results = new Dictionary<ServerTestResultsBase, string>();
            TestIDs = new Dictionary<int, string>();

            // Prepare the indexer which is needed for some object types
            var indexer = 0;

            // Create the list we will return later
            var results = new List<T>();

            // For each file in the directory
            foreach (var file in GetFileNames())
            {
                // Handle only binary files
                if (Path.GetExtension(file) != ".dat")
                    continue;

                // Get the object type
                var filecontent = new T();

                // Based on object type...
                if (filecontent is Test)
                {
                    // Convert file to T object
                    filecontent = GetObjectFromFile<T>(file, true);

                    // Set the ID of the test
                    (filecontent as Test).ID = indexer;

                    // Add it to the dictionary
                    TestIDs.Add(indexer, Path.GetFileNameWithoutExtension(file));

                    // Update the indexer
                    indexer++;
                }
                else if (filecontent is ServerTestResultsBase)
                {
                    // Convert file to T object
                    filecontent = GetObjectFromFile<T>(file);

                    // Add the object to the Results dictionary
                    Results.Add(filecontent as ServerTestResultsBase, Path.GetFileNameWithoutExtension(file));
                }
                else
                    // If no type found, just try to get the object as it is
                    filecontent = GetObjectFromFile<T>(file);

                // Add object to the list
                results.Add(filecontent);
            }

            // Finally return the list
            return results;
        }

        #endregion
    }
}

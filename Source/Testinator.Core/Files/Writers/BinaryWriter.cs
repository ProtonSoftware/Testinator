using System;
using System.IO;

namespace Testinator.Core
{
    /// <summary>
    /// The file writer which writes data in binary code
    /// </summary>
    public class BinaryWriter : FileManagerBase
    {
        #region Constructor

        /// <summary>
        /// Default constructor which requires to specify the type of object to write
        /// </summary>
        public BinaryWriter(SaveableObjects objectType)
        {
            ObjectType = objectType;
        }

        #endregion

        #region File Writing

        /// <summary>
        /// Writes the <see cref="Test"/> to the file
        /// </summary>
        /// <param name="test">The test to be written to file</param>
        public override void WriteToFile(Test test)
        {
            // Make sure we have test to write and the writer object type is set to test
            if (test == null || ObjectType != SaveableObjects.Test)
                throw new Exception("Wrong BinaryWriter usage.");

            // Create test file name
            var filename = BinaryReader.TestIDs.ContainsKey(test.ID) ? BinaryReader.TestIDs[test.ID] : CreateFinalFileName("Test");

            // Try to convert the test object to binary array
            if (!DataPackageDescriptor.TryConvertToBin(out var dataBin, new DataPackage(PackageType.TestForm, test)))
                throw new Exception("Test object is unconvertable.");

            // Finally save the file
            WriteBinaryDataToFile(filename, dataBin);

            // Reload the test list
            var binaryReader = new BinaryReader(SaveableObjects.Test);
            binaryReader.ReadFile<Test>();
        }

        /// <summary>
        /// Writes <see cref="ClientTestResults"/> to file
        /// </summary>
        public override void WriteToFile(ClientTestResults results)
        {
            // Make sure we have results to write and the writer object type is set to result
            if (results == null || ObjectType != SaveableObjects.Results)
                throw new Exception("Wrong BinaryWriter usage.");

            // Create the file name for this results
            var filenameFinal = CreateFinalFileName(CreateClientResultsFileName(results));

            // Try to convert the results object to binary array
            if (!DataPackageDescriptor.TryConvertToBin(out var dataBin, results))
                throw new Exception("Results object is unconvertable.");

            // Finally save the file
            WriteBinaryDataToFile(filenameFinal, dataBin);
        }

        /// <summary>
        /// Writes <see cref="TestResults"/> to file
        /// </summary>
        public override void WriteToFile(TestResults results)
        {
            // Make sure we have results to write and the writer object type is set to result
            if (results == null || ObjectType != SaveableObjects.Results)
                throw new Exception("Wrong BinaryWriter usage.");

            // Create the file name for this results
            var filenameFinal = CreateFinalFileName(CreateResultsFileName(results));

            // Try to convert the results object to binary array
            if (!DataPackageDescriptor.TryConvertToBin(out var dataBin, results))
                throw new Exception("Results object is unconvertable.");

            // Finally save the file
            WriteBinaryDataToFile(filenameFinal, dataBin);
        }

        #endregion

        #region File Deletion

        /// <summary>
        /// Deletes the <see cref="Test"/>
        /// </summary>
        /// <param name="test">The test to be deleted</param>
        public override void DeleteFile(Test test)
        {
            // Make sure we have test to delete and the writer object type is set to test
            if (test == null || ObjectType != SaveableObjects.Test)
                throw new Exception("Wrong BinaryWriter usage.");

            // If there is no test like passed one, don't delete anything
            if (!BinaryReader.TestIDs.ContainsKey(test.ID))
                return;

            // Try to delete test by filename
            var filename = BinaryReader.TestIDs[test.ID];
            if (!DeleteBinaryFileByName(filename))
                // If something went wrong, throw an error
                throw new Exception("Cannot delete file!");
        }

        /// <summary>
        /// Deletes the <see cref="TestResults"/>
        /// </summary>
        /// <param name="result">Results to be deleted</param>
        public override void DeleteFile(TestResults result)
        {
            // Make sure we have results to delete and the writer object type is set to result
            if (result == null || ObjectType != SaveableObjects.Results)
                throw new Exception("Wrong BinaryWriter usage.");

            // If there is no results like passed one, don't delete anything
            if (!BinaryReader.Results.ContainsKey(result))
                return;
            
            // Try to delete results by filename
            var filename = BinaryReader.Results[result];
            if (!DeleteBinaryFileByName(filename))
                // If something went wrong, throw an error
                throw new Exception("Cannot delete file!");
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Tries to delete a file by it's name
        /// </summary>
        /// <param name="filename">Name of the file to delete</param>
        private bool DeleteBinaryFileByName(string filename)
        {
            try
            {
                // Try to delete file by it's name in folder based on current object type
                File.Delete(DefaultPath + FolderNameBasedOnObjectType + filename + ".dat");
            }
            catch
            {
                // If failed to delete
                return false;
            }

            // Operation succeeded
            return true;
        }

        /// <summary>
        /// Writes the binary data to file
        /// </summary>
        /// <param name="filename">The name of the file to write to</param>
        /// <param name="data">The binary data to write as a byte array</param>
        private bool WriteBinaryDataToFile(string filename, byte[] data)
        {
            // Open the file we want to write to
            using (var writer = new System.IO.BinaryWriter(File.Open(DefaultPath + FolderNameBasedOnObjectType + filename + ".dat", FileMode.Create)))
            {
                // Simply write the data to it
                writer.Write(data);
            }

            // Operation succeeded
            return true;
        }

        /// <summary>
        /// Creates final filename by appending integer number at the end
        /// </summary>
        /// <param name="filenameOriginal">The original filename which we base on</param>
        private string CreateFinalFileName(string filenameOriginal)
        {
            // Start counting from 1
            var suffix = 1;

            // Count how many files are already in the direction we are right now
            foreach (var file in GetFileNames())
                suffix++;

            // Return final string
            return filenameOriginal + "(" + suffix + ")";
        }

        /// <summary>
        /// Creates the file name for <see cref="ClientTestResults"/>
        /// </summary>
        private string CreateClientResultsFileName(ClientTestResults results)
        {
            var hours = DateTime.Now.ToShortTimeString().Replace(':', '-');
            var date = DateTime.Now.ToShortDateString().Replace('/', '-');
            return results.ClientModel.ClientName + "-" + results.ClientModel.ClientSurname + "-" + date + "-" + hours;
        }

        /// <summary>
        /// Creates the file name for <see cref="TestResults"/>
        /// </summary>
        private string CreateResultsFileName(TestResults result)
        {
            var hours = result.Date.ToShortTimeString().Replace(':', '-');
            var date = result.Date.ToShortDateString().Replace('/', '-');
            return "Result" + "-" + date + "-" + hours;
        }

        #endregion
    }
}

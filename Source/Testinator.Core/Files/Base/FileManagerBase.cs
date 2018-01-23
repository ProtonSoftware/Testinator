using System;
using System.Collections.Generic;
using System.IO;

namespace Testinator.Core
{
    /// <summary>
    /// The base file manager settings for any reader or writer
    /// </summary>
    public abstract class FileManagerBase
    {
        #region Protected Properties
        
        /// <summary>
        /// The path the file manager will be handling
        /// </summary>
        protected string DefaultPath { get; set; }

        /// <summary>
        /// The type of object that file manager manages
        /// </summary>
        protected SaveableObjects ObjectType { get; set; }

        /// <summary>
        /// Returns the folder name based on <see cref="ObjectType"/>
        /// </summary>
        protected string FolderNameBasedOnObjectType
        {
            get
            {
                switch (ObjectType)
                {
                    case SaveableObjects.Config:
                        {
                            // Make sure the directory exists
                            Directory.CreateDirectory(DefaultPath + "Config\\");

                            // Then return its name
                            return "Config\\";
                        }
                    case SaveableObjects.Grading:
                        {
                            // Make sure the directory exists
                            Directory.CreateDirectory(DefaultPath + "Criteria\\");

                            // Then return its name
                            return "Criteria\\";
                        }
                    case SaveableObjects.Test:
                        {
                            // Make sure the directory exists
                            Directory.CreateDirectory(DefaultPath + "Tests\\");

                            // Then return its name
                            return "Tests\\";
                        }
                    case SaveableObjects.Results:
                        {
                            // Make sure the directory exists
                            Directory.CreateDirectory(DefaultPath + "Results\\");

                            // Then return its name
                            return "Results\\";
                        }

                    default:
                        return string.Empty;
                }
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FileManagerBase(Paths path = Paths.MyDocuments)
        {
            // Create the path
            switch (path)
            {
                case Paths.None:
                    DefaultPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    break;

                case Paths.MyDocuments:
                    DefaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Testinator\\";
                    break;

                case Paths.InstallationFolder:
                    DefaultPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Testinator\\";
                    break;
            }
        }

        #endregion

        #region File Writing

        /// <summary>
        /// Writes the <see cref="Test"/> to the file
        /// </summary>
        public virtual void WriteToFile(Test test) { }

        /// <summary>
        /// Writes the <see cref="GradingPercentage"/> to the file
        /// </summary>
        public virtual void WriteToFile(string filename, GradingPercentage grading) { }

        /// <summary>
        /// Writes logs to the file
        /// </summary>
        public virtual void WriteToFile(string text, string path, bool append = true) { }

        /// <summary>
        /// Writes <see cref="TestResults"/> to file
        /// </summary>
        public virtual void WriteToFile(TestResults tr) { }

        /// <summary>
        /// Writes <see cref="ClientTestResults"/> to file
        /// </summary>
        public virtual void WriteToFile(ClientTestResults ctr) { }

        /// <summary>
        /// Writes object's property info to file
        /// </summary>
        public virtual void WriteToFile(SettingsPropertyInfo property, bool fileExists = true) { }

        /// <summary>
        /// Writes every view model's property to the file
        /// </summary>
        /// <param name="vm">The view model with properties to save</param>
        public virtual void WriteToFile(BaseViewModel vm) { }

        #endregion

        #region File Reading

        /// <summary>
        /// Reads every files in the directory and creates <see cref="T"/> objects from them
        /// </summary>
        /// <typeparam name="T">The type of an object to get from files</typeparam>
        /// <returns>List of T objects</returns>
        public virtual List<T> ReadFile<T>()
            where T : class, new()
        { return null; }

        #endregion

        #region File Deletion

        /// <summary>
        /// Deletes the <see cref="Test"/>
        /// </summary>
        public virtual void DeleteFile(Test test) { }

        /// <summary>
        /// Deletes the <see cref="TestResults"/>
        /// </summary>
        public virtual void DeleteFile(TestResults results) { }

        #endregion

        #region Protected Helpers

        /// <summary>
        /// Replaces any / with \
        /// </summary>
        /// <param name="path">The path to normalize</param>
        protected string NormalizePath(string path) => path?.Replace('/', '\\').Trim();

        /// <summary>
        /// Resolves any relative elements of the path to absolute
        /// </summary>
        /// <param name="path">The path to resolve</param>
        protected string ResolvePath(string path) => Path.GetFullPath(path);

        /// <summary>
        /// Returns the list of names of every file in current directory
        /// </summary>
        protected List<string> GetFileNames() => new List<string>(Directory.GetFiles(DefaultPath + FolderNameBasedOnObjectType));

        /// <summary> 
        /// Gets the <see cref="T"/> object from file
        /// </summary>
        /// <param name="filename">The file name</param>
        /// <param name="shouldReturnDataPackage">Indicates whether to return T object or DataPackage object</param>
        protected T GetObjectFromFile<T>(string filename, bool shouldReturnDataPackage = false)
             where T : class, new()
        {
            // Get the byte array from specified array
            var byteArray = File.ReadAllBytes(filename);

            // If we need to return T object
            if (!shouldReturnDataPackage)
            {
                // Try to convert to T object
                if (DataPackageDescriptor.TryConvertToObj<T>(byteArray, out var data))
                    // Return if successful
                    return data as T;
            }
            // Otherwise...
            else
            {
                // Try to convert to DataPackage object
                if (DataPackageDescriptor.TryConvertToObj<DataPackage>(byteArray, out var data))
                    // Return if successful
                    return data.Content as T;
            }

            // Can't convert the file
            return null;
        }

        #endregion
    }
}

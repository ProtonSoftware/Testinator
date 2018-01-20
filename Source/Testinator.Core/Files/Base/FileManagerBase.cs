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
        #region Public Properties
        
        /// <summary>
        /// The path the file manager will be handling
        /// </summary>
        public string DefaultPath { get; set; }

        /// <summary>
        /// The type of object that file manager manages
        /// </summary>
        public SaveableObjects ObjectType { get; set; }

        /// <summary>
        /// Returns the folder name based on <see cref="ObjectType"/>
        /// </summary>
        public string FolderNameBasedOnObjectType
        {
            get
            {
                switch (ObjectType)
                {
                    case SaveableObjects.Config: return "Config\\";
                    case SaveableObjects.Grading: return "Criteria\\";
                    case SaveableObjects.Test: return "Tests\\";
                    case SaveableObjects.Results: return "Results\\";

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

            Directory.CreateDirectory(DefaultPath + "Config\\");
            Directory.CreateDirectory(DefaultPath + "Criteria\\");
            Directory.CreateDirectory(DefaultPath + "Tests\\");
            Directory.CreateDirectory(DefaultPath + "Results\\");
        }

        #endregion

        #region Protected Helpers

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

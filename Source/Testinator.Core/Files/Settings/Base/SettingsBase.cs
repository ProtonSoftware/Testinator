using System;
using System.Diagnostics;
using System.IO;

namespace Testinator.Core
{
    /// <summary>
    /// Base settings for any reader or writer
    /// </summary>
    public abstract class SettingsBase
    {
        #region Public Properties
        
        /// <summary>
        /// The path the files will be saved at
        /// </summary>
        public string Path { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SettingsBase(Paths path = Paths.MyDocuments)
        {
            switch (path)
            {
                case Paths.None:
                    Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    break;

                case Paths.MyDocuments:
                    Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Testinator\\";
                    break;

                case Paths.InstallationFolder:
                    Path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Testinator\\";
                    break;
            }

            Directory.CreateDirectory(Path + "Criteria\\");
            Directory.CreateDirectory(Path + "Tests\\");
        }

        #endregion
    }
}

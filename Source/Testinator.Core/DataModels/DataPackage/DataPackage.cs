using System;

namespace Testinator.Core
{
    /// <summary>
    /// Defines a package that is send between client and server
    /// </summary>
    [Serializable]
    public class DataPackage
    {
        #region Public Properties

        /// <summary>
        /// The type of this package
        /// </summary>
        public PackageType PackageType { get; set; }

        /// <summary>
        /// The content of this package
        /// </summary>
        public PackageContent Content { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a data package with the given type,
        /// Content is set to null by default
        /// </summary>
        /// <param name="Type">The type of this package</param>
        public DataPackage(PackageType Type)
        {
            PackageType = Type;
            Content = null;
        }

        /// <summary>
        /// Creates a data package from the given data
        /// </summary>
        /// <param name="Type">The type of this package</param>
        /// <param name="Content">The content of this package</param>
        public DataPackage(PackageType Type, PackageContent Content)
        {
            PackageType = Type;
            this.Content = Content;
        }

        #endregion

        #region Quick Creation Methods

        /// <summary>
        /// Creates a package that contains a test
        /// </summary>
        /// <param name="Test"></param>
        /// <returns></returns>
        public static DataPackage TestPackage(Test Test) => new DataPackage(PackageType.TestForm, Test);

        /// <summary>
        /// Creates a package that contains startup command with args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static DataPackage StartTestPackage(TestStartupArgs args) => new DataPackage(PackageType.BeginTest, args);

        /// <summary>
        /// Creates a package that contains abort test command
        /// </summary>
        public static DataPackage AbortTestPackage => new DataPackage(PackageType.StopTestForcefully);

        #endregion
    }
}
namespace Testinator.Core
{
    /// <summary>
    /// Setting for any reader
    /// </summary>
    public class ReaderSettings : SettingsBase
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ReaderSettings()
        {  }

        /// <summary>
        /// Constructs the settings for the reader 
        /// </summary>
        /// <param name="path">Where the files should be looked for, my documments by default</param>
        public ReaderSettings(Paths path = Paths.MyDocuments) : base(path)
        {  }

        #endregion
    }

    /// <summary>
    /// Settings for any writer
    /// </summary>
    public class WriterSettings : SettingsBase
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public WriterSettings()
        { }

        /// <summary>
        /// Constructs the settings for the writer 
        /// </summary>
        /// <param name="path">Where the files should be saved, my documments by default</param>
        public WriterSettings(Paths path = Paths.MyDocuments) : base(path)
        { }

        #endregion
    }
}

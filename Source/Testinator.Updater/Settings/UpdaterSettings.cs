namespace Testinator.Updater
{
    /// <summary>
    /// The settings of this updater
    /// </summary>
    public static class UpdaterSettings
    {
        /// <summary>
        /// Type of an application which requests an update
        /// </summary>
        public static ApplicationType AppType { get; set; }

        /// <summary>
        /// The application view model as a static member to locate it
        /// </summary>
        public static ApplicationViewModel ApplicationViewModel => IoCUpdater.Application;
    }
}

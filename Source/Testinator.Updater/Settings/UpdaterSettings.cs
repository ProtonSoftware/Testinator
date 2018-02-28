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
        /// The progress of downloading the update (%)
        /// </summary>
        public static int Progress { get; set; }
    }
}

using System.IO;

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
        /// The path to download installer to
        /// As default, current user temp folder
        /// </summary>
        public static string InstallerPath { get; private set; } = Path.GetTempPath();

        /// <summary>
        /// The application view model as a static member to locate it
        /// </summary>
        public static ApplicationViewModel ApplicationViewModel => IoCUpdater.Application;
    }
}

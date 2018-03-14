using System;
using System.IO;
using System.Net;
using Testinator.Core;

namespace Testinator.Updater
{
    /// <summary>
    /// The view model for main update download page
    /// </summary>
    public class DownloadPageViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Indicates how much of a progress is done on downloading as percentage
        /// </summary>
        public int Progress { get; set; } = 0;

        /// <summary>
        /// If any error occures, show this message
        /// </summary>
        public string ErrorMessage { get; private set; } = "";

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public DownloadPageViewModel()
        {
            // Start downloading from the start
            using (var wc = new WebClient())
            {
                // Everytime download progress changes, match our progress bar value
                wc.DownloadProgressChanged += (s, e) => Progress = e.ProgressPercentage;

                // Fire when download is completed
                wc.DownloadDataCompleted += DownloadCompleted;

                // Prepare variables
                var url = CreateInstallerUrl();
                var path = Path.GetTempPath();

                try
                {
                    // Try to download the installer file
                    wc.DownloadFileAsync(new Uri(url), path + "installer.msi");
                }
                catch
                {
                    // Can't download file, output error
                    ErrorMessage = "Error";
                }
            }
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Creates an url to the installer that we want to download
        /// </summary>
        private string CreateInstallerUrl()
        {
            // Get app type as string from settings
            var appType = UpdaterSettings.AppType.ToString();

            // Create url from that
            return "http://minorsonek.pl/testinator/data/Installer_" + appType + "/Testinator." + appType + "-1.1.0.0-x86.msi";
        }

        /// <summary>
        /// Fired when new version download is completed
        /// </summary>
        private void DownloadCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            // Change the page
            IoCUpdater.Application.GoToPage(ApplicationPage.Exit);
        }

        public void Install()
        {
            //var type = Type.GetTypeFromProgID("WindowsInstaller.Installer");
            //var installer = (Installer)Activator.CreateInstance(type);
            //installer.InstallProduct("YourPackage.msi");
        }

        #endregion
    }
}

using System;
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

                // Download the installer file
                wc.DownloadFileAsync(new Uri("http://minorsonek.pl/testinator/data/Installer_Server/Testinator.Server-1.1.0.0-x86.msi"), "\\temp\\installer.msi");
            }
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Fired when new version download is completed
        /// </summary>
        private void DownloadCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            
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

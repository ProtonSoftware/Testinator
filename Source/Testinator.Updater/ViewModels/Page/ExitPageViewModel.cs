using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Updater
{
    /// <summary>
    /// The view model for exit page
    /// </summary>
    public class ExitPageViewModel : BaseViewModel
    {
        #region Commands

        /// <summary>
        /// The command to exit the updater and do nothing
        /// </summary>
        public ICommand ExitAppCommand { get; private set; }

        /// <summary>
        /// The command to exit the updater and open the newly updated Testinator app
        /// </summary>
        public ICommand OpenUpdatedAppCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ExitPageViewModel()
        {
            // Create commands
            ExitAppCommand = new RelayCommand(ExitAndInstall);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Exits the updater and runs an installer
        /// </summary>
        public void ExitAndInstall()
        {
            // Run the installer downloaded in temp folder
            Process.Start(UpdaterSettings.InstallerPath + "installer.msi");

            // Exit this application
            Application.Current.Shutdown();
        }

        #endregion
    }
}

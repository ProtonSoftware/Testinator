using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Client.Core
{
    /// <summary>
    /// The view model for initial login page
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Name of app's user specified at the start
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Surname of app's user specified at the start
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// IP of the server we are connecting to
        /// </summary>
        public string ServerIP { get; set; } = "197.129.102.70";

        /// <summary>
        /// Port of the server we are connecting to
        /// </summary>
        public string ServerPort { get; set; } = "3333";

        /// <summary>
        /// Indicates if settings menu is opened
        /// </summary>
        public bool IsSettingsMenuOpened { get; set; } = false;

        /// <summary>
        /// A flag indicating if the connect command is running
        /// </summary>
        public bool ConnectionIsRunning { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to connect client app with the server
        /// </summary>
        public ICommand TryConnectingCommand { get; private set; }

        /// <summary>
        /// The command to expand the settings menu
        /// </summary>
        public ICommand SettingsMenuExpandCommand { get; private set; }

        /// <summary>
        /// The command to hide the settings menu
        /// </summary>
        public ICommand SettingsMenuHideCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginViewModel()
        {
            // Create commands
            TryConnectingCommand = new RelayCommand(async () => await Connect());
            SettingsMenuExpandCommand = new RelayCommand(ExpandMenu);
            SettingsMenuHideCommand = new RelayCommand(HideMenu);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Attempts to connect with the server
        /// </summary>
        private async Task Connect()
        {
            await RunCommandAsync(() => ConnectionIsRunning, async () =>
            {
                // TODO: Fake a connect...
                await Task.Delay(1000);

                // Go to next page
                //IoCClient.Application.GoToPage(ApplicationPage.Something);
            });
        }

        /// <summary>
        /// Expands the settings menu
        /// </summary>
        private void ExpandMenu()
        {
            // Simply togle the expanded menu flag
            IsSettingsMenuOpened = true;
        }

        private void HideMenu()
        {
            // Simply togle the expanded menu flag
            IsSettingsMenuOpened = false;
        }

        #endregion
    }
}

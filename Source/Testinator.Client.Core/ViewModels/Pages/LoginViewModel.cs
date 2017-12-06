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

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginViewModel()
        {
            // Create commands
            TryConnectingCommand = new RelayCommand(async () => await Connect());
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

        #endregion
    }
}

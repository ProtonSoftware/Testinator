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
        public string Name
        {
            get => IoCClient.Client.ClientName;
            set
            {
                IoCClient.Client.ClientName = value;
            }
        }

        /// <summary>
        /// Surname of app's user specified at the start
        /// </summary>
        public string Surname
        {
            get => IoCClient.Client.ClientSurname;
            set
            {
                IoCClient.Client.ClientSurname = value;
            }
        }

        /// <summary>
        /// IP of the server we are connecting to
        /// </summary>
        public string ServerIP { get; set; } = "192.168.1.14";

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
        public bool ConnectingIsRunning => IoCClient.Application.Network.Connecting;

        /// <summary>
        /// A flag indicating if the not valid data error should be shown
        /// </summary>
        public bool ErrorShouldBeShown { get; set; }

        /// <summary>
        /// A flag indicating if server port or ip is incorrect
        /// </summary>
        public bool IpOrPortError { get; set; }

        /// <summary>
        /// Number of attempts taken to connect to the server
        /// </summary>
        public int Attempts => IoCClient.Application.Network.Attempts;

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

        /// <summary>
        /// The command to stop connecting to the server
        /// </summary>
        public ICommand StopConnectingCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginViewModel()
        {
            // Create commands
            TryConnectingCommand = new RelayCommand(Connect);
            SettingsMenuExpandCommand = new RelayCommand(ExpandMenu);
            SettingsMenuHideCommand = new RelayCommand(HideMenu);
            StopConnectingCommand = new RelayCommand(StopConnecting);

            IoCClient.Application.Network.OnAttemptUpdate += Network_OnAttemptUpdate;
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Attempts to connect with the server
        /// </summary>
        private void Connect()
        {

            // Disable errors if something was shown before
            ErrorShouldBeShown = false;

            // If input data isn't valid, show an error and don't try to connect
            if (!IsInputDataValid())
            {
                ErrorShouldBeShown = true;
                return;
            }
            
            // Setup client and start connecting
            IoCClient.Application.Network.Initialize(ServerIP, Int32.Parse(ServerPort));
            IoCClient.Application.Network.StartConnecting();

            OnPropertyChanged(nameof(ConnectingIsRunning));
        }

        /// <summary>
        /// Expands the settings menu
        /// </summary>
        private void ExpandMenu()
        {
            // Dont show the menu if connecting is running
            if (ConnectingIsRunning)
                return;

            // Simply togle the expanded menu flag
            IsSettingsMenuOpened = true;
        }

        private void HideMenu()
        {
            // Verify the data
            if (!NetworkHelpers.IsAddresCorrect(ServerIP) || !NetworkHelpers.IsPortCorrect(ServerPort))
            {
                IpOrPortError = true;
                return;
            }

            // Simply togle the expanded menu flag
            IsSettingsMenuOpened = false;
        }

        /// <summary>
        /// Stops connecting to the server
        /// </summary>
        private void StopConnecting()
        {
            IoCClient.Application.Network.Disconnect();
            OnPropertyChanged(nameof(ConnectingIsRunning));
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Fired when attempt counter updates
        /// </summary>
        private void Network_OnAttemptUpdate()
        {
            // Update the view
            OnPropertyChanged(nameof(Attempts));
        }

        /// <summary>
        /// Validates the user's input data
        /// </summary>
        /// <returns></returns>
        private bool IsInputDataValid()
        {
            // For now, check if user have specified at least two character for each input
            if (Name.Length < 2) return false;
            if (Surname.Length < 2) return false;
            
            return true;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            get => IoCClient.Client.Name;
            set => IoCClient.Client.Name = value;
        }

        /// <summary>
        /// Surname of app's user specified at the start
        /// </summary>
        public string Surname
        {
            get => IoCClient.Client.LastName;
            set => IoCClient.Client.LastName = value;
        }

        /// <summary>
        /// IP of the server we are connecting to
        /// </summary>
        public string ServerIP { get; set; } = IoCClient.Application.Network.IPString;

        /// <summary>
        /// Port of the server we are connecting to
        /// </summary>
        public string ServerPort { get; set; } = IoCClient.Application.Network.Port.ToString();

        /// <summary>
        /// Indicates if settings menu is opened
        /// </summary>
        public bool IsSettingsMenuOpened { get; set; } = false;

        /// <summary>
        /// A flag indicating if the connect command is running
        /// </summary>
        public bool ConnectingIsRunning => IoCClient.Application.Network.IsTryingToConnect;

        /// <summary>
        /// If any error occur, show this message
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// A flag indicating if server port or ip is incorrect
        /// </summary>
        public bool IpOrPortError { get; set; }

        /// <summary>
        /// Indicates if the current connecting is being canceled right now
        /// </summary>
        public bool IsCancelling { get; set; }

        /// <summary>
        /// Content of the connect button based on <see cref="IsCancelling"/> property
        /// </summary>
        public string CancelText => IsCancelling ? LocalizationResource.Aborting + "..." : LocalizationResource.Cancel;

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

        /// <summary>
        /// Fired when website link is clicked
        /// </summary>
        public ICommand LinkClickedCommand { get; private set; }

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
            LinkClickedCommand = new RelayCommand(LinkClicked);

            IoCClient.Application.Network.AttemptCounterUpdated += Network_OnAttemptUpdate;
            IoCClient.Application.Network.AttemptsTimeout += Network_AttemptsTimeout;
            IoCClient.Application.Network.ConnectionFinished += Network_ConnectionFinished;
            IoCClient.Application.Network.Connected += Network_Connected;
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Attempts to connect with the server
        /// </summary>
        private void Connect()
        {
            // Disable errors if something was shown before
            ErrorMessage = "";

            // If input data isn't valid, show an error and don't try to connect
            if (!IsInputDataValid())
            {
                ErrorMessage = "Wprowadzone dane są niepoprawne.";
                return;
            }
            
            // Setup client and start connecting
            IoCClient.Application.Network.Initialize(ServerIP, int.Parse(ServerPort));
            IoCClient.Application.Network.Connect();
            
            // Log it
            IoCClient.Logger.Log("Attempting to connect to the server");

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

        /// <summary>
        /// Hides the settings menu
        /// </summary>
        private void HideMenu()
        {
            // Verify the data
            if (!NetworkHelpers.IsIPAddressCorrect(ServerIP) || !NetworkHelpers.IsPortCorrect(ServerPort))
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
            // Disconnect
            IoCClient.Application.Network.Disconnect();

            // Log it
            IoCClient.Logger.Log("User disconnected");

            IsCancelling = true;
        }

        /// <summary>
        /// Opens web browser and goes to the testinatro website
        /// </summary>
        private void LinkClicked()
        {
            Process.Start("http://www.google.com/");
        }


        #endregion

        #region Private Helpers

        private void Network_Connected()
        {
            // Unsubscribe from events, if the page changed to waiting for test this viewmodel gets destroyed,
            // but references to the event methods are stored in Network class so when the login page viewmodel get created again
            // event methods are subscribed again and, thus they are fired multiple times
            IoCClient.Application.Network.AttemptCounterUpdated -= Network_OnAttemptUpdate;
            IoCClient.Application.Network.AttemptsTimeout -= Network_AttemptsTimeout;
            IoCClient.Application.Network.ConnectionFinished -= Network_ConnectionFinished;
        }

        private void Network_ConnectionFinished()
        {
            OnPropertyChanged(nameof(ConnectingIsRunning));
            IsCancelling = false;
        }

        /// <summary>
        /// Fired when attempt counter updates
        /// </summary>
        private void Network_OnAttemptUpdate()
        {
            // Update the view
            OnPropertyChanged(nameof(Attempts));
        }

        /// <summary>
        /// Fired when attemts timeout is reached
        /// </summary>
        private void Network_AttemptsTimeout()
        {
            OnPropertyChanged(nameof(ConnectingIsRunning));
            IoCClient.UI.ShowMessage(new MessageBoxDialogViewModel()
            {
                Message = LocalizationResource.MaximumAttemptsReachedMessage,
                Title = LocalizationResource.ConnectionFalied,
                OkText = LocalizationResource.Ok,
            });
        }

        /// <summary>
        /// Validates the user's input data
        /// </summary>
        /// <returns></returns>
        private bool IsInputDataValid()
        {
            // For now, check if user have specified at least two character for each input
            if (string.IsNullOrEmpty(Name) || Name.Length < 2) return false;
            if (string.IsNullOrEmpty(Surname) || Surname.Length < 2) return false;
            
            return true;
        }

        #endregion
    }
}

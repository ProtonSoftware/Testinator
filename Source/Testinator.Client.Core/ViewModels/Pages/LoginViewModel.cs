using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
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

        /// <summary>
        /// Indicates if dark overlay should visible
        /// </summary>
        public bool OverlayVisible => ConnectingIsRunning || IsSettingsMenuOpened;

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
        /// The command to cancel changes in the settings menu
        /// </summary>
        public ICommand SettingsMenuCancelCommand { get; private set; }

        /// <summary>
        /// The command to load default values for IP an Port settings
        /// </summary>
        public ICommand SettingsMenuLoadDefaultValuesCommand { get; private set; }

        /// <summary>
        /// The command to submit changes in the settings menu
        /// </summary>
        public ICommand SettingsMenuSubmitCommand { get; private set; }

        /// <summary>
        /// The command to stop connecting to the server
        /// </summary>
        public ICommand StopConnectingCommand { get; private set; }

        /// <summary>
        /// Fired when website link is clicked
        /// </summary>
        public ICommand LinkClickedCommand { get; private set; }

        /// <summary>
        /// The command to handle the quote author link lick
        /// </summary>
        public ICommand QuoteAuthorClickedCommand { get; private set; }

        #endregion

        #region Construction/Desctruction
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginViewModel()
        {
            // Create commands
            TryConnectingCommand = new RelayCommand(Connect);
            SettingsMenuExpandCommand = new RelayCommand(ExpandMenu);
            SettingsMenuCancelCommand = new RelayCommand(SettingsMenuCancel);
            SettingsMenuSubmitCommand = new RelayCommand(SettingsMenuSubmit);
            StopConnectingCommand = new RelayCommand(StopConnecting);
            LinkClickedCommand = new RelayCommand(TestinatorWebLinkClicked);
            SettingsMenuLoadDefaultValuesCommand = new RelayCommand(SettingsMenuLoadDefaultValues);
            QuoteAuthorClickedCommand = new RelayCommand(QuoteAuthorClicked);

            IoCClient.Application.Network.AttemptCounterUpdated += Network_OnAttemptUpdate;
            IoCClient.Application.Network.AttemptsTimeout += Network_AttemptsTimeout;
            IoCClient.Application.Network.ConnectionFinished += Network_ConnectionFinished;
            PropertyChanged += LoginViewModel_PropertyChanged;
        }

        /// <summary>
        /// Frees resources
        /// </summary>
        public override void Dispose()
        {
            IoCClient.Application.Network.AttemptCounterUpdated -= Network_OnAttemptUpdate;
            IoCClient.Application.Network.AttemptsTimeout -= Network_AttemptsTimeout;
            IoCClient.Application.Network.ConnectionFinished -= Network_ConnectionFinished;
            PropertyChanged -= LoginViewModel_PropertyChanged;
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
            OnPropertyChanged(nameof(OverlayVisible));
        }

        /// <summary>
        /// Expands the settings menu
        /// </summary>
        private void ExpandMenu()
        {
            // Dont show the menu if connecting is running
            if (ConnectingIsRunning)
                return;

            // Menu already is expanded so no need to do anything
            if (IsSettingsMenuOpened)
                return;

            // Load initial values
            ServerIP = IoCClient.Application.Network.IPString;
            ServerPort = IoCClient.Application.Network.Port.ToString();

            // Simply expand menu
            IsSettingsMenuOpened = true;
        }

        /// <summary>
        /// Submits changes in settings menu
        /// </summary>
        private void SettingsMenuSubmit()
        {
            // Verify the data
            if (!NetworkHelpers.IsIPAddressCorrect(ServerIP) || !NetworkHelpers.IsPortCorrect(ServerPort))
            {
                IpOrPortError = true;
                return;
            }

            // Hide the menu
            IsSettingsMenuOpened = false;
        }

        /// <summary>
        /// Cancels all changes in settings menu
        /// </summary>
        private void SettingsMenuCancel()
        {
            // Just hide
            IsSettingsMenuOpened = false;
        }

        /// <summary>
        /// Reads default values from file or if that falis goes with defaults
        /// </summary>
        private void SettingsMenuLoadDefaultValues()
        {
            // No idea where to put this, maybe somewhere in file classes but after they are 
            // redone beacuse now they look AWFUL (no offence, but it's better way to do them)

            // Get directory in appdata
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "//Testinator//";

            var ip = default(IPAddress);
            var port = 0;

            try
            {
                // Try to read data from file
                var fileContent = File.ReadAllText(directory + "ipconfig.txt").Trim();
                var separatorIndex = fileContent.IndexOf(';');

                if (separatorIndex == -1)
                    throw new Exception();

                var ipString = fileContent.Substring(0, separatorIndex);
                var portString = fileContent.Substring(separatorIndex + 1);

                ip = IPAddress.Parse(ipString);

                if (!NetworkHelpers.IsPortCorrect(portString))
                    throw new Exception();

                port = int.Parse(portString);
            }
            catch
            {
                // If somwthing went wrong get deault values
                ip = IPAddress.Parse("127.1.1.0");
                port = 3333;
            }

            ServerIP = ip.ToString();
            ServerPort = port.ToString();
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
        /// Goes to the quote author website
        /// </summary>
        private void QuoteAuthorClicked()
        {
            // 'Pustka egzystencjalna' youtube channel
            OpenWebBrowserWithUrl("https://www.youtube.com/channel/UCJZLnUCOiQ9hfXwahvLv0SQ");
        }

        /// <summary>
        /// Goes to the testinator website
        /// </summary>
        private void TestinatorWebLinkClicked()
        {
            OpenWebBrowserWithUrl("http://testinator.minorsonek.pl/");
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Fired when attempting to connect finishes, with whatever result
        /// </summary>
        private void Network_ConnectionFinished()
        {
            OnPropertyChanged(nameof(ConnectingIsRunning));
            OnPropertyChanged(nameof(OverlayVisible));
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
            OnPropertyChanged(nameof(OverlayVisible));
            IoCClient.UI.ShowMessage(new MessageBoxDialogViewModel()
            {
                Message = LocalizationResource.MaximumAttemptsReachedMessage,
                Title = LocalizationResource.ConnectionFalied,
                OkText = LocalizationResource.Ok,
            });
        }

        /// <summary>
        /// Opens web browser with specified url to go to
        /// </summary>
        /// <param name="url">The website to go to</param>
        private void OpenWebBrowserWithUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            // No need to handle this, browse will not open thats all 
            // TODO: maybe we can notify it, we'll see
            catch { }
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

        /// <summary>
        /// Property changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // If thease properties are changed dont do anything
            if (e.PropertyName == nameof(ErrorMessage) || e.PropertyName == nameof(IpOrPortError))
                return;

            // Hide error message. Ex. error is displayed, user reads it and then starts typing something else so the error disapears
            
            if (!string.IsNullOrEmpty(ErrorMessage))
                ErrorMessage = "";

            if (IpOrPortError)
                IpOrPortError = false;
        }

        #endregion
    }
}

using System;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The view model for initial login page
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// User's input PIN
        /// </summary>
        public string PIN { get; set; }

        /// <summary>
        /// Repeated PIN from user's input, should match the original PIN
        /// </summary>
        public string RepeatedPIN { get; set; }

        /// <summary>
        /// Indicates if saved PIN number was found on the computer
        /// </summary>
        public bool PINFound { get; private set; }

        /// <summary>
        /// Indicates if user clicked submit button and is currently logging in
        /// </summary>
        public bool LoggingIsRunning { get; private set; } = false;

        /// <summary>
        /// The message to show if any error occures
        /// </summary>
        public string ErrorMessage { get; private set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to log the user in by PIN
        /// </summary>
        public ICommand LoginCommand { get; private set; }

        /// <summary>
        /// The command to create brand new PIN
        /// </summary>
        public ICommand CreatePINCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginViewModel()
        {
            // Create commands
            LoginCommand = new RelayCommand(Login);
            CreatePINCommand = new RelayCommand(CreatePIN);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Creates new PIN by storing it in hashed file
        /// </summary>
        private void CreatePIN()
        {
            // Check if both PIN matches
            if (!DoesRepeatedPINMatch())
            {
                ErrorMessage = "Repeated PIN is not the same as the original.";
                return;
            }

            // Save the PIN
            FileDataHasher.HashAndSaveString(PIN);

            FileDataHasher.ReadAndUnhashString();
        }

        /// <summary>
        /// Tries to log the user in by provided PIN
        /// </summary>
        private void Login()
        {
            
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Checks if both input PINs match themselves
        /// </summary>
        private bool DoesRepeatedPINMatch() => PIN == RepeatedPIN;

        #endregion
    }
}

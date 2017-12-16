using System;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Client.Core
{
    /// <summary>
    /// The view model for test result page 
    /// </summary>
    public class ResultViewModel : BaseViewModel
    {
        #region Public Commands

        /// <summary>
        /// Goes back to the waiting for test screen
        /// </summary>
        public ICommand GoBackCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ResultViewModel()
        {
            // Create commands
            GoBackCommand = new RelayCommand(GoBack);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Goes back to the waiting for test screen
        /// </summary>
        private void GoBack()
        {
            IoCClient.Application.GoToPage(ApplicationPage.WaitingForTest);
        }
        
        #endregion
    }
}

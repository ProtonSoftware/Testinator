using System.Collections.ObjectModel;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The view model for the begin test page
    /// </summary>
    public class BeginTestViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// A list of connected clients
        /// </summary>
        public ObservableCollection<ClientModel> ClientsConnected { get; set; }

        // TODO: Fix that
        //public BasePage CurrentTestPage { get; set; }

        /// <summary>
        /// A flag indicating whether server has started
        /// </summary>
        public bool IsServerStarted { get; set; }

        /// <summary>
        /// A flag indicating whether test has been sent to the clients
        /// </summary>
        public bool IsTestSent { get; set; }

        /// <summary>
        /// A flag indicating whether test has started
        /// </summary>
        public bool IsTestStarted { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to start the server (allows clients to connect)
        /// </summary>
        public ICommand StartServerCommand { get; private set; }

        /// <summary>
        /// The command to stop the server (disable client connections)
        /// </summary>
        public ICommand StopServerCommand { get; private set; }

        /// <summary>
        /// The command to change subpage to test list page
        /// </summary>
        public ICommand ChangePageTestListCommand { get; private set; }

        /// <summary>
        /// The command to change subpage to test info page
        /// </summary>
        public ICommand ChangePageTestInfoCommand { get; private set; }

        /// <summary>
        /// The command to choose a test from the list
        /// </summary>
        public ICommand ChooseTestCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BeginTestViewModel()
        {
            // Create commands
            StartServerCommand = new RelayCommand(() => IsServerStarted = true);
            StopServerCommand = new RelayCommand(() => IsServerStarted = false);
            //ChangePageTestListCommand = new RelayCommand();
            //ChangePageTestListCommand = new RelayCommand();
            ChangePageTestListCommand = new RelayCommand(ChooseTest);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Chooses a test from the list
        /// </summary>
        private void ChooseTest()
        {

        }

        #endregion
    }
}

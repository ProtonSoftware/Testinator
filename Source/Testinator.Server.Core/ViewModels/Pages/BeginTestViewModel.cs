using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
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
        public ObservableCollection<ClientModel> ClientsConnected { get; set; } = new ObservableCollection<ClientModel>();

        /// <summary>
        /// The test which is choosen by user on the list
        /// </summary>
        public Test CurrentTest { get; set; }

        /// <summary>
        /// A flag indicating whether server has started
        /// </summary>
        public bool IsServerStarted => IoCServer.Network.IsRunning;

        /// <summary>
        /// A flag indicating whether test has been sent to the clients
        /// </summary>
        public bool IsTestSent { get; set; }

        /// <summary>
        /// A flag indicating whether test has started
        /// </summary>
        public bool IsTestStarted => IoCServer.Application.IsTestInProgress;

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

        /// <summary>
        /// The command to start choosen test
        /// </summary>
        public ICommand BeginTestCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BeginTestViewModel()
        {
            // Create commands
            StartServerCommand = new RelayCommand(StartServer);
            StopServerCommand = new RelayCommand(StopServer);
            ChangePageTestListCommand = new RelayCommand(ChangePageList);
            ChangePageTestInfoCommand = new RelayCommand(ChangePageInfo);
            ChooseTestCommand = new RelayParameterizedCommand((param) => ChooseTest(param));
            BeginTestCommand = new RelayCommand(BeginTest);

            // Subscribe to the server events
            IoCServer.Network.OnClientConnected += ServerClientConnected;
            IoCServer.Network.OnClientDisconnected += ServerClientDisconnected;

            // Load every test from files
            TestListViewModel.Instance.LoadItems();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Starts the server
        /// </summary>
        private void StartServer()
        {
            IoCServer.Network.Start();
            OnPropertyChanged(nameof(IsServerStarted));

            // Prepare for client connections
            ClientsConnected = new ObservableCollection<ClientModel>();
        }

        /// <summary>
        /// Stops the server
        /// </summary>
        private void StopServer()
        {
            IoCServer.Network.Stop();
            OnPropertyChanged(nameof(IsServerStarted));
        }

        /// <summary>
        /// Changes the subpage to test choose page
        /// </summary>
        private void ChangePageList()
        {
            // Simply go to target page
            IoCServer.Application.GoToBeginTestPage(ApplicationPage.BeginTestChoose);
        }

        /// <summary>
        /// Changes the subpage to test info page
        /// </summary>
        private void ChangePageInfo()
        {
            // Check if user has choosen any test
            if (CurrentTest == null)
                return;

            // Then go to info page
            IoCServer.Application.GoToBeginTestPage(ApplicationPage.BeginTestInfo);
        }

        /// <summary>
        /// Chooses a test from the list
        /// </summary>
        private void ChooseTest(object param)
        {
            // Cast the parameter
            int testID = Int32.Parse(param.ToString());

            // Load test based on that
            CurrentTest = TestListViewModel.Instance.Items[testID-1];
        }

        /// <summary>
        /// Starts the test
        /// </summary>
        private void BeginTest()
        {
            
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Fired when a new user connectes to the server
        /// </summary>
        /// <param name="client">A model for this hte client</param>
        private void ServerClientConnected(ClientModel client)
        {
            // Jump on the dispatcher thread
            var uiContext = SynchronizationContext.Current;
            uiContext.Send(x => ClientsConnected.Add(client), null);
        }

        /// <summary>
        /// Fired when a client disconnected from the server
        /// </summary>
        /// <param name="client">The client that has disconnected</param>
        private void ServerClientDisconnected(ClientModel client)
        {           
            // Jump on the dispatcher thread
            var uiContext = SynchronizationContext.Current;
            uiContext.Send(x => ClientsConnected.Remove(client), null);
        }
        #endregion
    }
}

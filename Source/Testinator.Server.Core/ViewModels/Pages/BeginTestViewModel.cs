using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Timers;
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
        public ObservableCollection<ClientModel> ClientsConnected => IoCServer.Network.Clients;

        /// <summary>
        /// All clients that are currently taking the test
        /// </summary>
        public ObservableCollection<ClientModelExtended> ClientsTakingTheTest => IoCServer.TestHost.Clients;

        /// <summary>
        /// The test which is choosen by user on the list
        /// </summary>
        public Test CurrentTest => IoCServer.TestHost.Test;

        /// <summary>
        /// A flag indicating whether server has started
        /// </summary>
        public bool IsServerStarted => IoCServer.Network.IsRunning;

        /// <summary>
        /// A flag indicating whether test has started
        /// </summary>
        public bool IsTestInProgress => IoCServer.TestHost.IsTestInProgress;

        /// <summary>
        /// The time that is left to the end of the test
        /// </summary>
        public TimeSpan TimeLeft => IoCServer.TestHost.TimeLeft;

        /// <summary>
        /// The server's ip
        /// </summary>
        public string ServerIpAddress { get; set; } = IoCServer.Network.Ip;

        /// <summary>
        /// The server's port
        /// </summary>
        public string ServerPort { get; set; } = IoCServer.Network.Port.ToString();

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
        /// The command to start choosen test
        /// </summary>
        public ICommand BeginTestCommand { get; private set; }

        /// <summary>
        /// The command to stop the test
        /// </summary>
        public ICommand StopTestCommand { get; private set; }

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
            BeginTestCommand = new RelayCommand(BeginTest);
            StopTestCommand = new RelayCommand(StopTest);

            // Load every test from files
            TestListViewModel.Instance.LoadItems();

            // Hook to timer event
            IoCServer.TestHost.OnTimerUpdated += TimerUpdated;
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Starts the server
        /// </summary>
        private void StartServer()
        {
            // If ip and port is not valid, dont start the server
            if (!NetworkHelpers.IsAddressCorrect(ServerIpAddress) && !NetworkHelpers.IsPortCorrect(ServerPort))
                return;

            // Set network data
            IoCServer.Network.Ip = ServerIpAddress;
            IoCServer.Network.Port = int.Parse(ServerPort);

            // Start the server
            IoCServer.Network.Start();

            // Inform the view
            OnPropertyChanged(nameof(IsServerStarted));
        }

        /// <summary>
        /// Stops the server
        /// </summary>
        private void StopServer()
        {
            if (IoCServer.TestHost.IsTestInProgress)
            {
                // TODO: show dialog asking the user that a test is in progress
                // Based on the response decide what to do
                // if (response == nieWyłączaj)
                return;
            }
            
            // Stop the server
            IoCServer.Network.Stop();

            // Inform the view
            OnPropertyChanged(nameof(IsServerStarted));

            // Go to the initial page
            IoCServer.Application.GoToBeginTestPage(ApplicationPage.BeginTestInitial);
        }

        /// <summary>
        /// Changes the subpage to test choose page
        /// </summary>
        private void ChangePageList()
        {
            // If server isnt started, dont change the page
            if (!IsServerStarted)
                // TODO: Show message to the user
                return;

            // Simply go to target page
            IoCServer.Application.GoToBeginTestPage(ApplicationPage.BeginTestChoose);
        }

        /// <summary>
        /// Changes the subpage to test info page
        /// </summary>
        private void ChangePageInfo()
        {
            // Check if user has choosen any test
            if (!TestListViewModel.Instance.IsAnyTestSelected())
                return;

            // Then go to info page
            IoCServer.Application.GoToBeginTestPage(ApplicationPage.BeginTestInfo);

            // Meanwhile lock the clients list and send them the test 
            IoCServer.TestHost.LockClients();
            IoCServer.TestHost.SendTest();
        }

        /// <summary>
        /// Starts the test
        /// </summary>
        private void BeginTest()
        {
            IoCServer.TestHost.Start();
            IoCServer.Application.GoToBeginTestPage(ApplicationPage.BeginTestInProgress);
        }

        /// <summary>
        /// Stops the test
        /// </summary>
        private void StopTest()
        {
            IoCServer.TestHost.Stop();
            IoCServer.Application.GoToBeginTestPage(ApplicationPage.BeginTestInitial);
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Fired when timeleft timer is updated
        /// </summary>
        private void TimerUpdated()
        {
            // Update the view
            OnPropertyChanged(nameof(TimeLeft));
        }

        #endregion
    }
}

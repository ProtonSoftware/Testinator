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
            ChooseTestCommand = new RelayParameterizedCommand((param) => ChooseTest(param));
            BeginTestCommand = new RelayCommand(BeginTest);
            StopTestCommand = new RelayCommand(StopTest);

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
            OnPropertyChanged(nameof(IsServerStarted));

            // Go to the initial page
            IoCServer.Application.GoToBeginTestPage(ApplicationPage.BeginTestInitial);
        }

        /// <summary>
        /// Changes the subpage to test choose page
        /// </summary>
        private void ChangePageList()
        {
            if (IsServerStarted)
            {
                // Simply go to target page if the server has beed started
                IoCServer.Application.GoToBeginTestPage(ApplicationPage.BeginTestChoose);
            }
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
        /// Chooses a test from the list
        /// </summary>
        private void ChooseTest(object param)
        {
            // Cast the parameter
            var testID = int.Parse(param.ToString());
            
            // Load test based on that
            IoCServer.TestHost.BindTest(TestListViewModel.Instance.Items[testID - 1].Test);
            
            // Mark all items not selected
            foreach (var item in TestListViewModel.Instance.Items)
            {
                item.IsSelected = false;
            }

            // Select the one that has been clicked
            TestListViewModel.Instance.Items[testID - 1].IsSelected = true;
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
        }

        #endregion
    }
}

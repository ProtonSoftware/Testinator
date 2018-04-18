using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public ObservableCollection<ClientModel> ClientsConnected => IoCServer.Network.ConnectedClients;

        /// <summary>
        /// All clients that are currently taking the test
        /// </summary>
        public ObservableCollection<ClientModel> ClientsTakingTheTest => IoCServer.TestHost.ClientsInTest;

        /// <summary>
        /// The test which is choosen by user on the list
        /// </summary>
        public Test CurrentTest => IoCServer.TestHost.CurrentTest;

        /// <summary>
        /// The number of connected clients
        /// </summary>
        public int ClientsNumber => IoCServer.Network.ConnectedClientsCount;

        /// <summary>
        /// The number of the questions in the test
        /// </summary>
        public int QuestionsCount => CurrentTest.Questions.Count;

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
        public string ServerIpAddress => IoCServer.Network.IPString;

        /// <summary>
        /// The server's port
        /// </summary>
        public string ServerPort { get; set; } = IoCServer.Network.Port.ToString();

        /// <summary>
        /// Indicates if the result page should be allowed for the users
        /// </summary>
        public bool ResultPageAllowed { get; set; } = true;

        /// <summary>
        /// Indicates if the test should be held in fullscreen mode
        /// </summary>
        public bool FullScreenMode { get; set; } = false;

        #region Error flags

        /// <summary>
        /// Indicates if there is not enough clients to start the test
        /// </summary>
        public bool NotEnoughClients => ClientsNumber == 0;

        /// <summary>
        /// Indicates if the test is not selected
        /// </summary>
        public bool TestNotSelected => !TestListViewModel.Instance.IsAnySelected;

        /// <summary>
        /// Indicates if the test can be sent to the clients
        /// </summary>
        public bool CanSendTest => !NotEnoughClients && !TestNotSelected;

        #endregion

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
        /// The command to stop the test (test disappears completely)
        /// </summary>
        public ICommand StopTestCommand { get; private set; }
        
        /// <summary>
        /// The command to finish the test (results are collected before a timer ends)
        /// </summary>
        public ICommand FinishTestCommand { get; private set; }

        /// <summary>
        /// The command to exit from the resultpage
        /// </summary>
        public ICommand ResultPageExitCommand { get; private set; }

        /// <summary>
        /// The command to add a latecommer to the current test session
        /// </summary>
        public ICommand AddLateComerCommand { get; private set; }

        #endregion

        #region Construction/Desctruction

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
            FinishTestCommand = new RelayCommand(FinishTest);
            ResultPageExitCommand = new RelayCommand(ResultPageExit);
            AddLateComerCommand = new RelayCommand(AddLatecomers);

            // Load every test from files
            TestListViewModel.Instance.LoadItems();

            // Keep the view up-to-date
            IoCServer.TestHost.OnTimerUpdated += () => UpdateView();
            IoCServer.Network.OnClientConnected += (s) => UpdateView();
            IoCServer.Network.OnClientDisconnected += (s) => UpdateView();
            TestListViewModel.Instance.SelectionChanged += () => UpdateView();

            // Hook to the test host evet
            IoCServer.TestHost.TestFinished += ChangePageToResults;
        }

        /// <summary>
        /// Disposes this viewmodel
        /// </summary>
        public override void Dispose()
        {
            // Unsub from events
            IoCServer.TestHost.OnTimerUpdated -= () => UpdateView();
            IoCServer.Network.OnClientConnected -= (s) => UpdateView();
            IoCServer.Network.OnClientDisconnected -= (s) => UpdateView();
            TestListViewModel.Instance.SelectionChanged -= () => UpdateView();
            IoCServer.TestHost.TestFinished -= ChangePageToResults;
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Adds latecomer to the current test session
        /// </summary>
        private void AddLatecomers()
        {
            var CanStartTestClients = GetPossibleTestStartingClients();

            if (CanStartTestClients.Count == 0)
            {
                IoCServer.UI.ShowMessage(new MessageBoxDialogViewModel()
                {
                    Message = "No latecomers to add!",
                    OkText = "OK",
                    Title = "Adding users to the test",
                });
            }
            else
            {
                var viewmodel = new AddLatecomersDialogViewModel(CanStartTestClients)
                {
                    Title = "Adding users to the test",
                    Message = "Select users that you want to add to the current test",
                    AcceptText = "Add",
                    CancelText = "Cancel",
                };

                IoCServer.UI.ShowMessage(viewmodel);

                if (viewmodel.UserResponse.Count != 0)
                    IoCServer.TestHost.AddLateComers(viewmodel.UserResponse);
            }
        }

        /// <summary>
        /// Starts the server
        /// </summary>
        private void StartServer()
        {
            // If port is not valid, dont start the server
            if (!NetworkHelpers.IsPortCorrect(ServerPort))
            {
                // Show a message box with info about it
                IoCServer.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Niepoprawne dane!",
                    Message = "Niepoprawny port.",
                    OkText = "Ok"
                });
                return;
            }

            // Set network data
            IoCServer.Network.Port = int.Parse(ServerPort);

            // Start the server
            IoCServer.Network.Start();

            UpdateView();
        }

        /// <summary>
        /// Stops the server
        /// </summary>
        private void StopServer()
        {
            // Check if any test is already in progress
            if (IoCServer.TestHost.IsTestInProgress)
            {
                // Ask the user if he wants to stop the test
                var vm = new DecisionDialogViewModel()
                {
                    Title = "Test w trakcie!",
                    Message = "Test jest w trakcie. Czy chcesz go przerwać?",
                    AcceptText = "Tak",
                    CancelText = "Nie",
                };
                IoCServer.UI.ShowMessage(vm);
                
                // If he agreed
                if (vm.UserResponse)
                    // Stop the test
                    StopTestForcefully();
                else
                    return;

                UpdateView();
            }
            
            // Stop the server
            IoCServer.Network.ShutDown();

            UpdateView();

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
            {
                // Show a message box with info about it
                IoCServer.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Serwer nie włączony!",
                    Message = "By zmienić stronę, należy przedtem włączyć serwer.",
                    OkText = "Ok"
                });
                return;
            }

            // Simply go to target page
            IoCServer.Application.GoToBeginTestPage(ApplicationPage.BeginTestChoose);
        }

        /// <summary>
        /// Changes the subpage to test info page
        /// </summary>
        private void ChangePageInfo()
        {
            // Meanwhile lock the clients list and send them the test 
            IoCServer.TestHost.AddClients(IoCServer.Network.ConnectedClients.ToList());

            // Add selected test
            IoCServer.TestHost.AddTest(TestListViewModel.Instance.SelectedItem);

            // If there is no enough users to start the test, show the message and don't send the test
            if (IoCServer.TestHost.ClientsInTest.Count == 0)
            {
                IoCServer.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Brak użytkowników",
                    Message = "Nie można ropocząć testu. Brak użytkowników, którzy mogą go rozpocząć.",
                    OkText = "OK"
                });

                return;
            }

            IoCServer.TestHost.SendTestToAll();

            // Then go to the info page
            IoCServer.Application.GoToBeginTestPage(ApplicationPage.BeginTestInfo);
        }

        /// <summary>
        /// Starts the test
        /// </summary>
        private void BeginTest()
        {
            // Send the args before startting
            IoCServer.TestHost.ConfigureStartup(new TestOptions()
            {
                FullScreenEnabled = FullScreenMode,
                ResultsPageAllowed = ResultPageAllowed,
            });

            IoCServer.TestHost.TestStart();
            IoCServer.Application.GoToBeginTestPage(ApplicationPage.BeginTestInProgress);
        }

        /// <summary>
        /// Stops the test (test disappears completely, like it didn't even happened)
        /// </summary>
        private void StopTest()
        {
            // Ask the user if he wants to stop the test
            var vm = new DecisionDialogViewModel()
            {
                Title = "Przerywanie testu",
                Message = "Czy na pewno chcesz przerwać test?",
                AcceptText = "Tak",
                CancelText = "Nie",
            };
            IoCServer.UI.ShowMessage(vm);

            // If his will match
            if (vm.UserResponse)
                // Stop the test
                StopTestForcefully();
        }

        /// <summary>
        /// Finishes the test before a timer ends, results are collected here
        /// </summary>
        private void FinishTest()
        {
            // TODO: Get the results from users (even empty ones)
            //       Then finish the test etc.
        }

        /// <summary>
        /// Exits from the result page
        /// </summary>
        private void ResultPageExit()
        {
            // Go back to the main begin test page
            IoCServer.Application.GoToPage(ApplicationPage.BeginTest);

            // Change the mini-page accordingly
            if (IoCServer.Network.IsRunning)
                IoCServer.Application.GoToBeginTestPage(ApplicationPage.BeginTestChoose);
            else
                IoCServer.Application.GoToBeginTestPage(ApplicationPage.BeginTestInitial);
        }

        #endregion

        #region Private Event Methods

        /// <summary>
        /// Fired when the test finishes
        /// </summary>
        private void ChangePageToResults()
        {
            // Change page on UI Thread
            IoCServer.UI.DispatcherThreadAction(() => IoCServer.Application.GoToPage(ApplicationPage.BeginTestResults));
        }

        /// <summary>
        /// Updates the view and all the properties
        /// </summary>
        private void UpdateView()
        {
            OnPropertyChanged(nameof(ClientsNumber));
            OnPropertyChanged(nameof(NotEnoughClients));
            OnPropertyChanged(nameof(CanSendTest));
            OnPropertyChanged(nameof(TestNotSelected));
            OnPropertyChanged(nameof(CanSendTest));
            OnPropertyChanged(nameof(TimeLeft));
            OnPropertyChanged(nameof(TestNotSelected));
            OnPropertyChanged(nameof(IsServerStarted));
        }

        /// <summary>
        /// Stops the test forcefully
        /// </summary>
        private void StopTestForcefully()
        {
            IoCServer.TestHost.AbortTest();
            IoCServer.Application.GoToBeginTestPage(ApplicationPage.BeginTestInitial);
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Gets all the clients that can possibly start a test
        /// </summary>
        /// <returns>All the clients that can start a test right now</returns>
        private List<ClientModel> GetPossibleTestStartingClients()
        {
            var CanStartTestClients = new List<ClientModel>();

            foreach(var client in IoCServer.Network.ConnectedClients)
                if (client.CanStartTest)
                    CanStartTestClients.Add(client);

            return CanStartTestClients;
        }

        #endregion
    }
}
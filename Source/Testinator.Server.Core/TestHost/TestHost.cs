using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Testinator.Core;
using System.Timers;
using System.Threading;

namespace Testinator.Server.Core
{
    /// <summary>
    /// Hosts the test at server side
    /// </summary>
    public class TestHost : BaseViewModel
    {
        #region Private Members 

        /// <summary>
        /// Timer to handle cutdown
        /// </summary>
        private System.Timers.Timer mTestTimer = new System.Timers.Timer(1000);

        #endregion

        #region Public Properties

        /// <summary>
        /// The test that is currently hosted
        /// </summary>
        public Test CurrentTest { get; private set; }

        /// <summary>
        /// The results binary file writer which handles results saving/deleting from local folder
        /// </summary>
        public BinaryWriter ResultsFileWriter { get; private set; } = new BinaryWriter(SaveableObjects.Results);

        /// <summary>
        /// Indicates if there is any test in progress
        /// </summary>
        public bool IsTestInProgress { get; private set; }

        /// <summary>
        /// Indicates if test host is currently waiting for results 
        /// </summary>
        public bool WaitingForResults { get; private set; }

        /// <summary>
        /// The time left for this test
        /// </summary>
        public TimeSpan TimeLeft { get; private set; } = default(TimeSpan);

        /// <summary>
        /// All clients that are currently taking the test
        /// </summary>
        public ObservableCollection<ClientModel> ClientsInTest { get; private set; } = new ObservableCollection<ClientModel>();

        /// <summary>
        /// The user results for the current test
        /// </summary>
        public ServerTestResults Results { get; private set; }

        /// <summary>
        /// Stores the lastest startup args
        /// </summary>
        public TestStartupArgs TestStartupArgs { get; private set; }

        /// <summary>
        /// Current session identifier
        /// </summary>
        public Guid CurrentSessionIdentifier { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts the test
        /// </summary>
        public void TestStart()
        {
            if (IsTestInProgress)
                return;

            if (CurrentTest == null || TestStartupArgs == null || ClientsInTest == null || ClientsInTest.Count == 0)
                throw new Exception("Not ready to start test");
            
            // Send package indicating the start of the test
            var TestStartPackage = DataPackage.StartTestPackage(TestStartupArgs);

            foreach (var client in ClientsInTest)
            {
                // Send start command with args
                IoCServer.Network.Send(client, TestStartPackage);

                // Reset the client for new test
                client.ResetForNewTest(CurrentTest.Questions.Count);
            }

            IsTestInProgress = true;

            TimeLeft = CurrentTest.Info.Duration;

            // Start cutdown
            mTestTimer.Start();
            OnTimerUpdated.Invoke();
        }

        /// <summary>
        /// Aborts the currect test and without saving results
        /// </summary>
        public void AbortTest()
        {
            if (!IsTestInProgress)
                return;

            StopTimer();

            var stopTestPackage = DataPackage.AbortTestPackage;

            foreach (var client in ClientsInTest)
            {
                client.CanStartTest = true;
                IoCServer.Network.Send(client, stopTestPackage);
            }

            IsTestInProgress = false;

            CleanUp();
        }

        /// <summary>
        /// Configures current test session with the given options
        /// </summary>
        /// <param name="options">Defines options for current test session</param>
        public void ConfigureStartup(TestOptions options)
        {
            if (IsTestInProgress)
                return;

            if (CurrentSessionIdentifier.Equals(default(Guid)))
                CurrentSessionIdentifier = Guid.NewGuid();

            // If there were no args before create new ones
            if (TestStartupArgs == null)
            {
                TestStartupArgs = new TestStartupArgs()
                {
                    SessionIdentifier = CurrentSessionIdentifier,
                    FullScreenMode = options.FullScreenEnabled,
                    IsResultsPageAllowed = options.ResultsPageAllowed,
                    TimerOffset = TimeSpan.FromSeconds(0),
                };
            }
            // Or update existing ones
            else
            {
                TestStartupArgs.FullScreenMode = options.FullScreenEnabled;
                TestStartupArgs.IsResultsPageAllowed = options.ResultsPageAllowed;
            }
        }

        /// <summary>
        /// Adds latecomers to the current test session
        /// </summary>
        /// <param name="Latecomers">People to add</param>
        public void AddLateComers(List<ClientModel> Latecomers)
        {
            if (!IsTestInProgress)
                return;

            var packageWithTest = DataPackage.TestPackage(CurrentTest);
            var packageWithArgs = DataPackage.StartTestPackage(TestStartupArgs);
            
            // Based on the arguments set the timer offset
            TestStartupArgs.TimerOffset = CurrentTest.Info.Duration - TimeLeft;

            // Add clients to the test
            foreach (var client in Latecomers)
            {
                ClientsInTest.Add(client);

                IoCServer.Network.Send(client, packageWithTest);

                IoCServer.Network.Send(client, packageWithArgs);

                client.ResetForNewTest(CurrentTest.Questions.Count);
            }
        }

        /// <summary>
        /// Sends test to the clients
        /// </summary>
        public void SendTestToAll()
        {
            if (IsTestInProgress || CurrentTest == null)
                return;

            // Create the data package
            var dataPackage = DataPackage.TestPackage(CurrentTest);

            SendAllClients(dataPackage);
        }

        /// <summary>
        /// Sets the clients that will be taking the test
        /// </summary>
        /// <param name="ClientsToAdd">The clients that will take the test</param>
        public void AddClients(List<ClientModel> ClientsToAdd)
        {
            if (IsTestInProgress)
                // Use add latecommers method in this case
                return;

            // Clear the list 
            ClientsInTest.Clear();
            
            foreach (var client in ClientsToAdd)
            {
                if (client.CanStartTest)
                {
                    ClientsInTest.Add(client);
                }
            }
        }

        /// <summary>
        /// Adds a test object to this host
        /// </summary>
        /// <param name="Test"></param>
        public void AddTest(Test Test)
        {
            if (IsTestInProgress)
                return;

            CurrentTest = Test;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Fired when time left value is updated
        /// Used by viewmodels to update their values
        /// </summary>
        public event Action OnTimerUpdated = () => { };

        /// <summary>
        /// Fired when any data is resived from a client
        /// </summary>
        /// <param name="client">The sender client</param>
        /// <param name="dataPackage">The data received from the client</param>
        public void OnDataReceived(ClientModel client, DataPackage dataPackage)
        {
            // If the data is from client we dont care about don't do anything
            if (!ClientsInTest.Contains(client))
                return;

            switch (dataPackage.PackageType)
            {
                case PackageType.ReportStatus:

                    // Status package, contains only number of questions the client has done so far
                    var content = dataPackage.Content as StatusPackage;
                    client.CurrentQuestion = content.CurrentQuestion;

                    break;

                case PackageType.ReadyForTest:

                    client.CanStartTest = true;
                    break;

                case PackageType.ResultForm:
                    
                    // Get the content
                    var result = dataPackage.Content as ResultFormPackage;

                    // Save them in client model
                    client.Answers = result.Answers;
                    client.PointsScored = result.PointsScored;
                    client.Mark = result.Mark;
                    client.QuestionsOrder = result.QuestionsOrder;
                    client.HasResultsBeenReceived = true;

                    if (HasEveryClientSentResults())
                    {
                        SaveResults();
                        FinishTest();
                    }
                    
                    else if (OnlyClientsWithConnectionProblemLeft())
                    {
                        var vm = new DecisionDialogViewModel()
                        {
                            Title = "Finishing test",
                            Message = "Do you want to end the test before time as only users left are these with connection problem?",
                            AcceptText = "Ok",
                            CancelText = "No, wait for them",
                        };

                        IoCServer.UI.ShowMessage(vm);

                        // Stop before time
                        if (vm.UserResponse)
                        {
                            SaveResults();
                            FinishTest();
                        }
                    }

                    break;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestHost()
        {
            // Initialize timer
            mTestTimer.Elapsed += HandleTimer;

            IoCServer.Network.OnClientDataUpdated += ServerNetwork_OnClientDataUpdated;
            IoCServer.Network.OnDataReceived += OnDataReceived;
        }

        #endregion

        #region Private Events

        /// <summary>
        /// Fired when the client data is updated
        /// </summary>
        /// <param name="OldModel"></param>
        /// <param name="NewModel"></param>
        private void ServerNetwork_OnClientDataUpdated(ClientModel OldModel, ClientModel NewModel)
        {
            if (IsTestInProgress)
                return;

            // If the client is included in this test update it's model
            if (ClientsInTest.Contains(OldModel))
            {
                NewModel.ResetForNewTest(CurrentTest.Questions.Count);
                ClientsInTest[ClientsInTest.IndexOf(OldModel)] = NewModel;
            }
        }

        /// <summary>
        /// Fired when a client disconnected from the server
        /// </summary>
        /// <param name="client">The client that has disconnected</param>
        private void OnClientDisconnected(ClientModel client)
        {
            // If the client that has disconnected is the one who isn't taking the test right now, don't do anything
            if (!ClientsInTest.Contains(client))
                return;

            if (IsTestInProgress)
                client.HasConnectionProblem = true;
            else
                ClientsInTest.Remove(client);
        }

        /// <summary>
        /// Fired every time timer's cycle elapses
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleTimer(object sender, ElapsedEventArgs e)
        {
            TimeLeft = TimeLeft.Subtract(new TimeSpan(0, 0, 1));

            OnTimerUpdated.Invoke();

            if (TimeLeft.Equals(TimeSpan.Zero))
            {
                TimesUp();
            }
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Finished the test
        /// </summary>
        private void FinishTest()
        {
            IsTestInProgress = false;
            IoCServer.Application.GoToPage(ApplicationPage.BeginTestResults);

            // Clear guid
            CurrentSessionIdentifier = default(Guid);
        }

        /// <summary>
        /// Cleans the test host and prepares it for brand new usage
        /// </summary>
        private void CleanUp()
        {
            // Clean up
            CurrentTest = null;
            TestStartupArgs = null;
            TimeLeft = default(TimeSpan);
            Results = null;
            ClientsInTest.Clear();
            CurrentSessionIdentifier = default(Guid);
        }

        /// <summary>
        /// Checks if every user has sent results
        /// </summary>
        /// <returns></returns>
        private bool HasEveryClientSentResults()
        {
            foreach(var client in ClientsInTest)
            {
                if (client.HasResultsBeenReceived == false)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if only clients left are these with connection problems
        /// </summary>
        /// <returns></returns>
        private bool OnlyClientsWithConnectionProblemLeft()
        {
            // Indicates if there is still some users that are answering questions
            var someoneStillSolvingTheTest = false;

            var someoneWithConnectionProbles = false;

            foreach (var client in ClientsInTest)
            {
                // Not interested in those who have already sent results
                if (client.HasResultsBeenReceived)
                    continue;

                // If there is a client with connection problem indicates it
                if (client.HasConnectionProblem)
                    someoneWithConnectionProbles = true;
                else
                    // Or if there is a client thet doesnot have a connection probles
                    someoneStillSolvingTheTest = true;

            }

            return !someoneStillSolvingTheTest && someoneWithConnectionProbles;
        }

        /// <summary>
        /// Sends data to all clients
        /// </summary>
        /// <param name="data">The data to be sent</param>
        private void SendAllClients(DataPackage data)
        {
            // Send it to all clients
            foreach (var client in ClientsInTest)
                IoCServer.Network.Send(client, data);
        }

        /// <summary>
        /// Saves results to the file
        /// </summary>
        private void SaveResults()
        {
            Results = new ServerTestResults()
            {
                Date = DateTime.Now,
                Test = CurrentTest,
                SessionIdentifier = TestStartupArgs.SessionIdentifier,
            };

            foreach (var client in ClientsInTest)
            {
                Results.ClientAnswers.Add(new TestResultsClientModel(client)
                {
                    PointsScored = client.PointsScored,
                    Mark = client.Mark,
                    QuestionsOrder = client.QuestionsOrder,
                }, client.Answers);
            }

            ResultsFileWriter.WriteToFile(Results);
        }

        /// <summary>
        /// Stops the timer
        /// </summary>
        private void StopTimer()
        {
            mTestTimer.Start();
            OnTimerUpdated.Invoke();
        }

        /// <summary>
        /// Handles the time out
        /// </summary>
        private void TimesUp()
        {
            StopTimer();

            WaitingForResults = true;

            // Wait 3 seconds in case of any desynchro with clients
            Thread.Sleep(3000);
            WaitingForResults = false;

            SaveResults();

            FinishTest();
        }

        #endregion

    }
}

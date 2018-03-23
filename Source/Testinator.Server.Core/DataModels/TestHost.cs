using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Testinator.Core;
using System.Timers;

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
        private Timer mTestTimer = new Timer(1000);

        #endregion

        #region Public Properties

        /// <summary>
        /// The test that is currently hosted
        /// </summary>
        public Test Test { get; private set; } = new Test();

        /// <summary>
        /// The results binary file writer which handles results saving/deleting from local folder
        /// </summary>
        public BinaryWriter ResultsFileWriter { get; private set; } = new BinaryWriter(SaveableObjects.Results);

        /// <summary>
        /// Indicates if there is any test in progress
        /// </summary>
        public bool IsTestInProgress { get; private set; }

        /// <summary>
        /// The time left for this test
        /// </summary>
        public TimeSpan TimeLeft { get; private set; }

        /// <summary>
        /// All clients that are currently taking the test
        /// </summary>
        public ObservableCollection<ClientModel> Clients { get; private set; } = new ObservableCollection<ClientModel>();

        /// <summary>
        /// The user results for the current test
        /// </summary>
        public ServerTestResults Results { get; private set; } = new ServerTestResults();

        /// <summary>
        /// Stores the lastest startup args
        /// </summary>
        public TestStartupArgsPackage StartupArgs { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts the test
        /// </summary>
        public void TestStart()
        {
            if (IsTestInProgress)
                return;

            // Send package indicating the start of the test
            var data = new DataPackage(PackageType.BeginTest);
            SendAllClients(data);

            foreach (var client in Clients)
                client.CanStartTest = false;

            IsTestInProgress = true;

            TimeLeft = Test.Duration;

            // Start cutdown
            mTestTimer.Start();
            OnTimerUpdated.Invoke();
        }

        /// <summary>
        /// Stops the test
        /// </summary>
        public void TestStop()
        {
            if (!IsTestInProgress)
                return;

            IsTestInProgress = false;

            StopTimer();
        }

        /// <summary>
        /// Stops the test forcefully
        /// </summary>
        public void TestStopForcefully()
        {
            if (!IsTestInProgress)
                return;

            SendAllClients(new DataPackage(PackageType.StopTestForcefully));

            IsTestInProgress = false;

            StopTimer();
        }

        /// <summary>
        /// Adds latecomers to the current test session
        /// </summary>
        /// <param name="Latecomers">People to be added</param>
        public void AddLateComers(List<ClientModel> Latecomers)
        {
            foreach (var client in Latecomers)
            {
                Clients.Add(client);
            }

            SendTestRange(Latecomers);

            StartupArgs.TimerOffset = Test.Duration - TimeLeft;

            SendTestArgsRange(Latecomers);

            // Send package indicating the start of the test
            var data = new DataPackage(PackageType.BeginTest);
            System.Threading.Thread.Sleep(100);
            SendRange(data, Latecomers);

            foreach (var client in Clients)
                client.CanStartTest = false;
        }

        /// <summary>
        /// Sends the test args to the users
        /// </summary>
        /// <param name="args">The args</param>
        public void SendTestArgsToAll(TestStartupArgsPackage args)
        {
            if (args == null)
                return;

            // Save for any latecomers
            StartupArgs = args;

            SendAllClients(new DataPackage(PackageType.TestStartupArgs)
            {
                Content = args,
            });
        }

        /// <summary>
        /// Sends the test args to only specified users
        /// </summary>
        /// <param name="args">The args</param>
        /// <param name="SendTo">People to be sent the data</param>
        public void SendTestArgsRange(List<ClientModel> SendTo)
        {
            if (StartupArgs == null)
                return;

            SendRange(new DataPackage(PackageType.TestStartupArgs)
            {
                Content = StartupArgs,
            }, SendTo);
        }

        /// <summary>
        /// Sends test to the clients
        /// </summary>
        public void SendTestToAll()
        {
            if (IsTestInProgress || Test == null)
                return;

            // Create the data package
            var dataPackage = new DataPackage(PackageType.TestForm)
            {
                Content = Test,
            };

            SendAllClients(dataPackage);
        }

        /// <summary>
        /// Sends test only to specified clients
        /// </summary>
        /// <param name="SendTo"></param>
        public void SendTestRange(List<ClientModel> SendTo)
        {
            // Create the data package
            var dataPackage = new DataPackage(PackageType.TestForm)
            {
                Content = Test,
            };

            SendRange(dataPackage, SendTo);
        }

        /// <summary>
        /// Locks the client list that are currently taking the test
        /// </summary>
        public void LockClientsAll()
        {
            if (IsTestInProgress)
                return;

            // Save all currently connected clients
            Clients = new ObservableCollection<ClientModel>();
            foreach (var client in IoCServer.Network.ConnectedClients)
            {
                if (client.CanStartTest)
                {
                    Clients.Add(client);
                }
            }
        }

        /// <summary>
        /// Binds a test object to this host
        /// </summary>
        /// <param name="test"></param>
        public void BindTest(Test test)
        {
            if (IsTestInProgress)
                return;

            Test = test;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Fired when time left value is updated
        /// Used by viewmodels to update their values
        /// </summary>
        public event Action OnTimerUpdated = () => { };

        /// <summary>
        /// Fired when the test finishes
        /// </summary>
        public event Action TestFinished = () => { };

        /// <summary>
        /// Fired when any data is resived from a client
        /// </summary>
        /// <param name="client">The sender client</param>
        /// <param name="dataPackage">The data received from the client</param>
        public void OnDataReceived(ClientModel client, DataPackage dataPackage)
        {
            // If the data is from client we dont care about don't do anything
            if (!Clients.Contains(client))
                return;

            switch (dataPackage.PackageType)
            {
                case PackageType.ReportStatus:
                    var content = dataPackage.Content as StatusPackage;
                    client.CurrentQuestion = content.CurrentQuestion;

                    // Check if every user has completed the test
                    if(IsTestFinished())
                    {
                        // Stop the test
                        TestStop();
                    }
                    break;

                case PackageType.ResultForm:
                    
                    // Get the content
                    var result = dataPackage.Content as ResultFormPackage;

                    // Store the result
                    client.Answers = result.Answers;
                    client.PointsScored = result.PointsScored;
                    client.Mark = result.Mark;

                    // If the test is not in progress save the answers
                    if(!IsTestInProgress)
                    {
                        SaveResults();
                        TestFinished.Invoke();
                    }
                    break;
            }
        }

        /// <summary>
        /// Fired when a client disconnected from the server
        /// </summary>
        /// <param name="client">The client that has disconnected</param>
        public void OnClientDisconnected(ClientModel client)
        {
            // If the client that has disconnected is the one who isn't taking the test right now, don't do anything
            if (!Clients.Contains(client))
                return;

            // Indicate that we got connection problem with this client
            client.ConnectionProblem = true;
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
            // Lock the clients again
            LockClientsAll();
        }

        /// <summary>
        /// Fired every time timer's cycle elapses
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleTimer(object sender, ElapsedEventArgs e)
        {
            TimeLeft = TimeLeft.Subtract(new TimeSpan(0, 0, 1));
            if (TimeLeft.Equals(new TimeSpan(0, 0, 0)))
            {
                TimesUp();
            }
            OnTimerUpdated.Invoke();
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Checks if there is any user still taking the test
        /// </summary>
        /// <returns></returns>
        private bool IsTestFinished()
        {
            foreach (var client in Clients)
            {
                // Test is not finished when there is a client that has not answered all questions and has no connection problems 
                if (!client.ConnectionProblem && client.CurrentQuestion < Test.Questions.Count + 1)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Sends data to all clients
        /// </summary>
        /// <param name="data">The data to be sent</param>
        private void SendAllClients(DataPackage data)
        {
            // Send it to all clients
            foreach (var client in Clients)
                IoCServer.Network.Send(client, data);
        }

        /// <summary>
        /// Sends data only to specified clients
        /// </summary>
        /// <param name="data">The data to be sent</param>
        /// <param name="SendTo">Clients to whom the data should be sent</param>
        private void SendRange(DataPackage data, List<ClientModel> SendTo)
        {
            // Send it to all clients
            foreach (var client in SendTo)
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
                Test = Test,
            };

            foreach (var client in Clients)
                Results.ClientAnswers.Add(new TestResultsClientModel(client)
                {
                    PointsScored = client.PointsScored,
                    Mark = client.Mark,
                }, client.Answers);

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
            TestStop();
            SaveResults();
            TestFinished.Invoke();
        }

        #endregion

    }
}

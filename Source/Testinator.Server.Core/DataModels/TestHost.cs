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

        /// <summary>
        /// Private list of all clients that are taking the test, 
        /// NOTE: data to the clients is sent by using <see cref="ClientModel"/>, therefore this list is essential
        /// </summary>
        private List<ClientModel> mClients = new List<ClientModel>();

        /// <summary>
        /// The user results for the current test
        /// </summary>
        private TestResults mResults = new TestResults();
        
        #endregion

        #region Public Properties

        /// <summary>
        /// The test that is currently hosted
        /// </summary>
        public Test Test { get; private set; }

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
        public ObservableCollection<ClientModelExtended> Clients { get; private set; } = new ObservableCollection<ClientModelExtended>();

        /// <summary>
        /// The user results for the current test
        /// </summary>
        public TestResults Results => mResults;

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
            SendToAllClients(data);

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

            SendToAllClients(new DataPackage(PackageType.StopTestForcefully));

            IsTestInProgress = false;

            StopTimer();
        }

        /// <summary>
        /// Sends test to the clients
        /// </summary>
        public void SendTest()
        {
            if (IsTestInProgress || Test == null)
                return;

            // Create the data package
            var dataPackage = new DataPackage(PackageType.TestForm)
            {
                Content = Test,
            };

            SendToAllClients(dataPackage);
        }

        /// <summary>
        /// Locks the client list that are currently taking the test
        /// </summary>
        public void LockClients()
        {
            if (IsTestInProgress)
                return;

            // Save all currently connected clients
            Clients = new ObservableCollection<ClientModelExtended>();
            mClients = new List<ClientModel>();
            foreach (var client in IoCServer.Network.Clients)
            {
                mClients.Add(client);
                Clients.Add(new ClientModelExtended(client)
                {
                    QuestionsCount = Test.Questions.Count,
                });
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
            if (!mClients.Contains(client))
                return;

            var ClientIdx = mClients.IndexOf(client);
            switch (dataPackage.PackageType)
            {
                case PackageType.ReportStatus:
                    var content = dataPackage.Content as StatusPackage;
                    Clients[ClientIdx].CurrentQuestion = content.CurrentQuestion;

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
                    Clients[ClientIdx].Answers = result.Answers;
                    Clients[ClientIdx].PointsScored = result.PointsScored;
                    Clients[ClientIdx].Mark = result.Mark;

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
            if (!mClients.Contains(client))
                return;

            var idx = mClients.IndexOf(client);

            // Indicate that we got connection problem with this client
            Clients[idx].ConnectionProblem = true;
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
            LockClients();
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
        private void SendToAllClients(DataPackage data)
        {
            // Send it to all clients
            foreach (var client in mClients)
                IoCServer.Network.SendData(client, data);
        }

        /// <summary>
        /// Saves results to the file
        /// </summary>
        private void SaveResults()
        {
            mResults = new TestResults()
            {
                Date = DateTime.Now,
                Test = Test,
            };

            foreach (var client in Clients)
                mResults.Results.Add(new ClientModelSerializable(client), client.Answers);

            FileWriters.BinWriter.WriteToFile(mResults);
        }

        /// <summary>
        /// Stopes the timer
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

    }
}

using System;
using System.Collections.Generic;
using System.Timers;
using Testinator.Core;

namespace Testinator.Client.Core
{
    /// <summary>
    /// Responsible for hosting a test
    /// </summary>
    public class TestHost : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// Timer to handle cutdown
        /// </summary>
        private Timer mTestTimer = new Timer(1000);

        /// <summary>
        /// Indicates current question staring from 0
        /// </summary>
        private int mCurrentQuestion = 0;

        /// <summary>
        /// Indicates if this application is in full screen mode
        /// </summary>
        private bool mFullScreenModeOn = false;

        /// <summary>
        /// Indicates if this is the first question that test host is showing
        /// Prevent from question counter bug
        /// </summary>
        private bool mFirstQuestion = true;

        #endregion

        #region Public Properties

        /// <summary>
        /// The test that is currently hosted
        /// </summary>
        public Test CurrentTest { get; set; }

        /// <summary>
        /// List of all questions in the test
        /// </summary>
        public List<Question> Questions => CurrentTest.Questions;

        /// <summary>
        /// List of every answer given by the user throughout the test
        /// </summary>
        public List<Answer> UserAnswers { get; private set; }

        /// <summary>
        /// The results binary file writer which handles results saving/deleting from local folder
        /// </summary>
        public BinaryWriter ResultFileWriter { get; private set; } = new BinaryWriter(SaveableObjects.Results);

        /// <summary>
        /// Indicates if the test is in progress
        /// </summary>
        public bool IsTestInProgress { get; private set; }

        /// <summary>
        /// A flag indicating if we have any test to show,
        /// to show corresponding content in the WaitingPage
        /// </summary>
        public bool IsTestReceived { get; private set; }

        /// <summary>
        /// Indicated if the test result has been sucesfully sent for this session
        /// </summary>
        public bool IsResultSent { get; private set; }

        /// <summary>
        /// Indicates if the user has completed the test
        /// </summary>
        public bool IsTestCompleted => mCurrentQuestion > Questions.Count;

        /// <summary>
        /// Indicates how much time is left
        /// </summary>
        public TimeSpan TimeLeft { get; private set; }

        /// <summary>
        /// Indicates if the server app has allowed user to check his answers just after they finish the test
        /// </summary>
        public bool AreResultsAllowed { get; private set; }

        /// <summary>
        /// Indicates if the applicating is currently showing any of the result pages
        /// </summary>
        public bool IsShowingResultPage { get; private set; }

        /// <summary>
        /// Shows which question is currently shown
        /// </summary>
        public string CurrentQuestionString { get; private set; }

        /// <summary>
        /// The user's score
        /// </summary>
        public int UserScore { get; private set; }

        /// <summary>
        /// The user's mark
        /// </summary>
        public Marks UserMark { get; private set; }

        /// <summary>
        /// Indicates if the test should be held in fullscren mode
        /// </summary>
        public bool FullScreenMode
        {
            get => mFullScreenModeOn;
            private set
            {
                // Set the value
                mFullScreenModeOn = value;

                // Check if value indicates to run the full screen mode
                if (value) FullScreenModeOn.Invoke();

                // Or to disable it
                else FullScreenModeOff.Invoke();
            }
        }

        /// <summary>
        /// The viewmodels for the result page, contaning question, user answer and the correct answer
        /// </summary>
        public List<BaseViewModel> QuestionViewModels { get; set; } = new List<BaseViewModel>();

        #endregion

        #region Public Events

        /// <summary>
        /// Fired when a test is received
        /// </summary>
        public event Action OnTestReceived = () => { };

        /// <summary>
        /// Fired when server has sent a behest to run a full screen mode
        /// </summary>
        public event Action FullScreenModeOn = () => { };

        /// <summary>
        /// Fired after completing the test to escape from full screen mode
        /// </summary>
        public event Action FullScreenModeOff = () => { };

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestHost()
        {
            mTestTimer.Elapsed += HandleTimer;
        }

        #endregion

        #region Public Helpers

        /// <summary>
        /// Starts the test 
        /// </summary>
        public void StartTest()
        {
            // If there is no test to start or the test has already started or the result page is displayed don't do anything
            if (!IsTestReceived || IsTestInProgress || IsShowingResultPage)
                return;

            if (CurrentTest == null)
                throw new Exception("Cannot start the test");

            // Indicate that test is starting
            IoCClient.Logger.Log("Test is starting...");

            IsTestInProgress = true;

            // Initialize the answer list so user can add their answers to it
            UserAnswers = new List<Answer>();

            // Start the test timer
            mTestTimer.Start();

            // Show the first question
            GoNextQuestion();
        }

        /// <summary>
        /// Stops the test forcefully, if asked by the server
        /// </summary>
        public void AbortTest()
        {
            // Stop the test only if it is in progress
            if (!IsTestInProgress)
                return;

            IoCClient.Logger.Log("Test has been stopped forcefully");

            // Show a message box with info about it
            IoCClient.UI.ShowMessage(new MessageBoxDialogViewModel()
            {
                Title = "Test został zatrzymany!",
                Message = "Test został zatrzymany na polecenie serwera.",
                OkText = "Ok"
            });

            // Reset the test host
            Reset();

            // Return to the main screen
            IoCClient.Application.ReturnMainScreen();
        }

        /// <summary>
        /// Binds the test to this view model 
        /// </summary>
        /// <param name="test">Test to be hosted</param>
        public void BindTest(Test test)
        {
            // Don't do anything in this case
            if (IsTestInProgress || IsShowingResultPage)
                return;
            
            // Save the test
            CurrentTest = test;

            // Set the timeleft to the duration time
            TimeLeft = test.Info.Duration;

            // Randomize question order
            Questions.Shuffle();
            IoCClient.Logger.Log("Shuffling questions");

            // Indicate that we have received test
            IsTestReceived = true;
            OnTestReceived.Invoke();
        }

        /// <summary>
        /// Resets the test host and clears all the flags and properties
        /// </summary>
        public void Reset()
        {
            IoCClient.Logger.Log("Reseting test host...");

            ResetQuestionNumber();

            // Stop the timer
            mTestTimer.Stop();

            // Clear all properties
            CurrentTest = null;
            UserAnswers = new List<Answer>();
            UserScore = 0;
            UserMark = default(Marks);
            QuestionViewModels = new List<BaseViewModel>();

            // Clear flags
            IsTestInProgress = false;
            IsShowingResultPage = false;
            IsTestReceived = false;
            IsResultSent = false;
            AreResultsAllowed = true;
            FullScreenMode = false;

            IoCClient.Logger.Log("Reseting test host done.");
        }

        /// <summary>
        /// Saves the answer for the current question
        /// </summary>
        /// <param name="answer">The answer itself</param>
        public void SaveAnswer(Answer answer)
        {
            // Save the answer
            UserAnswers.Add(answer);

            // Log it
            IoCClient.Logger.Log($"User answer added for question nr {CurrentQuestionString}");
        }

        /// <summary>
        /// Goes to the next question, or
        /// shows the end screen if there is no more questions
        /// </summary>
        public void GoNextQuestion()
        {
            if (!mFirstQuestion)
            { 
                // Update question number
                UpdateQuestionNumber();

                // Send the update
                SendUpdate();
            }

            // If last question was the last question, finish the test
            if (mCurrentQuestion >= Questions.Count)
            {
                TestFinished();
                return;
            }

            // Indicate that we are going to the next question
            IoCClient.Logger.Log("Going to the next question");

            QuestionHelpers.ShowQuestion(CurrentTest.Questions[mCurrentQuestion]);

            mFirstQuestion = false;
        }

        /// <summary>
        /// Sets the test startup arguments send by the server
        /// </summary>
        /// <param name="args">The arguments to be set</param>
        public void SetupArguments(TestStartupArgs args)
        {
            AreResultsAllowed = args.IsResultsPageAllowed;
            FullScreenMode = args.FullScreenMode;
            TimeLeft = CurrentTest.Info.Duration - args.TimerOffset;
        }

        /// <summary>
        /// Compares the both answers and questions lists and calculates user point score based on that
        /// </summary>
        public void CalculateScore()
        {
            // Total point score
            var totalScore = 0;

            // Log what we are doing
            IoCClient.Logger.Log("Calculating user's score");

            for (var i = 0; i < CurrentTest.Questions.Count; i++)
            {
                // We can stop here as all the rest answers are blank
                if (UserAnswers[i] == null)
                    break;

                totalScore += CurrentTest.Questions[i].CheckAnswer(UserAnswers[i]);
            }

            // Set calculated point score to the property and set the corresponding mark
            UserScore = totalScore;
            UserMark = CurrentTest.Grading.GetMark(UserScore);
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Attempts to send the results to the server
        /// </summary>
        private void TrySendResult()
        {
            // Create the data package
            var data = new DataPackage(PackageType.ResultForm)
            {
                Content = new ResultFormPackage()
                {
                    Answers = UserAnswers,
                    PointsScored = UserScore,
                    Mark = UserMark,
                },
            };

            // Send or save the data
            if (IoCClient.Application.Network.IsConnected)
            {
                IoCClient.Application.Network.SendData(data);
                IsResultSent = true;
            }

            else
            {
                try
                {
                    // Try to write the results to file
                    ResultFileWriter.WriteToFile(new ClientTestResults()
                    {
                        Answers = UserAnswers,
                        Client = new TestResultsClientModel()
                        {
                            Name = IoCClient.Client.Name,
                            LastName = IoCClient.Client.LastName,
                            MachineName = IoCClient.Client.MachineName,
                            Mark = UserMark,
                            PointsScored = UserScore,
                        },
                        Test = CurrentTest,
                    });
                }
                catch (Exception ex)
                {
                    // If an error occured, show info to the user
                    IoCClient.UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Title = "Błąd zapisu",
                        Message = "Nie udało się zapisać ani wysłać wyników testu." +
                                  "\nTreść błędu: " + ex.Message,
                        OkText = "Ok"
                    });

                    IoCClient.Logger.Log("Unable to save the results, error message: " + ex.Message);

                    // Don't show the next message
                    return;
                }

                // Show a message box with info about it
                IoCClient.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Wyniki testu w pliku",
                    Message = "Wyniki testu zostały zapisane do pliku, ponieważ połączenie z serwerem zostało utracone.",
                    OkText = "Ok"
                });
            }
        }

        /// <summary>
        /// Fired when the current test is finished
        /// </summary>
        private void TestFinished()
        {
            // Calculate the user's score
            CalculateScore();

            // Try to send the results
            TrySendResult();

            // Test is not in progress now
            IsTestInProgress = false;

            // If full screen mode was fired, disable it
            if (FullScreenMode) FullScreenMode = false;

            // Change page to the result page
            IoCClient.UI.ChangePage(ApplicationPage.ResultOverviewPage);

            // Dont need the connection in the result page so stop reconnecting if meanwhile connection has been lost
            IoCClient.Application.Network.StopReconnecting();

            // Indicate that we're in the result page
            IsShowingResultPage = true;
        }

        /// <summary>
        /// Fired when the cutdown reaches 0
        /// </summary>
        private void TimesUp()
        {
            IoCClient.UI.ShowMessage(new MessageBoxDialogViewModel()
            {
                Title = "Koniec czasu",
                Message = "Czas przeznaczony na rozwiązanie testu minął!",
                OkText = "OK",
            });

            TestFinished();
        }

        /// <summary>
        /// Sends the update to the server
        /// </summary>
        private void SendUpdate()
        {
            // Create package
            var data = new DataPackage(PackageType.ReportStatus)
            { 
                Content = new StatusPackage()
                {
                    // Send progress user has made
                    CurrentQuestion = mCurrentQuestion,
                },
            };
            
            // Send it to the server
            IoCClient.Application.Network.SendData(data);

            // Log it
            IoCClient.Logger.Log("Sending progress package to the server");
        }

        /// <summary>
        /// Resets the question number
        /// </summary>
        private void ResetQuestionNumber()
        {
            mCurrentQuestion = 0;
            CurrentQuestionString = mCurrentQuestion + " / " + Questions.Count;
        }

        /// <summary>
        /// Updates the current question number
        /// </summary>
        private void UpdateQuestionNumber()
        {
            mCurrentQuestion++;
            CurrentQuestionString = mCurrentQuestion + 1 + " / " + Questions.Count;
        }

        /// <summary>
        /// Handles the cutdown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleTimer(object sender, ElapsedEventArgs e)
        {
            // Every second substract one second from time left property
            TimeLeft = TimeLeft.Subtract(new TimeSpan(0, 0, 1));

            // If we reach 0, time has run out and so the test
            if (TimeLeft.Equals(new TimeSpan(0, 0, 0)))
            {
                mTestTimer.Stop();
                TimesUp();        
            }
        }

        #endregion
    }
}

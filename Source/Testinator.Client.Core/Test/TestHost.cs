using System;
using System.Collections.Generic;
using Testinator.Core;
using System.Timers;

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
        private System.Timers.Timer mTestTimer = new System.Timers.Timer(1000);

        /// <summary>
        /// Indicates current question
        /// </summary>
        private int mCurrentQuestion = 0;

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
        /// Indicates if the applicating is currently showing any of the result pages
        /// </summary>
        public bool IsShowingResultPage { get; private set; }

        /// <summary>
        /// Shows which question is currently shown
        /// </summary>
        public string QuestionNumber { get; private set; }

        /// The user's score
        /// </summary>
        public int UserScore { get; private set; }

        /// <summary>
        /// The user's mark
        /// </summary>
        public Marks UserMark { get; private set; }

        /// <summary>
        /// The viewmodels for the result page, contaning question, user answer and the correct answer
        /// </summary>
        public List<BaseViewModel> QuestionViewModels { get; set; } = new List<BaseViewModel>();

        #endregion

        #region Public Events

        /// <summary>
        /// Fired when a test is receievd
        /// </summary>
        public event Action OnTestReceived = () => { };

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

            // Indicate that test is starting
            IoCClient.Logger.Log("Test is starting...");
            IsTestInProgress = true;

            // Initialize the answer list so user can add his answer to it
            UserAnswers = new List<Answer>();

            // Start the test timer
            mTestTimer.Start();

            // Show the first question
            GoNextQuestion();
        }

        /// <summary>
        /// Stopes the test forcefully, if asked by the server
        /// </summary>
        public void StopTestForcefully()
        {
            // Stop the test only if it is in progress
            if (IsTestInProgress)
            {
                IoCClient.Logger.Log("Test has beed stoped forcefully");
                
                // TODO: show a dialogbox with info about it

                Reset();
                
                // Return to the main screen
                IoCClient.Application.ReturnMainScreen();
            }
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
            TimeLeft = test.Duration;

            // Randomize question order
            Questions.Shuffle();
            IoCClient.Logger.Log("Shuffling questions");

            // Indicate that we have received test
            IsTestReceived = true;
            OnTestReceived.Invoke();
        }

        /// <summary>
        /// Resets the test host and clear all the flags and properties
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
            UserMark = Marks.F;
            QuestionViewModels = new List<BaseViewModel>();

            // Clear flags
            IsTestInProgress = false;
            IsShowingResultPage = false;
            IsTestReceived = false;
            IsResultSent = false;

            IoCClient.Logger.Log("Reseting test host done.");
        }

        /// <summary>
        /// Saves the answer for the current question
        /// </summary>
        /// <param name="answer">The answer itself</param>
        public void SaveAnswer(Answer answer)
        {
            // Make id of the question match the id of the answer
            answer.ID = Questions[mCurrentQuestion - 1].ID;

            // Save the answer
            UserAnswers.Add(answer);

            // Log it
            IoCClient.Logger.Log("Add user answer");
        }

        /// <summary>
        /// Goes to the next question, or
        /// Starts the test, or
        /// Shows the end screen if there is no more questions
        /// </summary>
        public void GoNextQuestion()
        {
            // Update question number
            UpdateQuestionNumber();

            // Send the update
            SendUpdate();

            // If last question was the last question, finish the test
            if (mCurrentQuestion > Questions.Count)
            {
                TestFinished();
                return;
            }

            // Indicate that we are going to the next question
            IoCClient.Logger.Log("Going to the next question");

            // Based on next question type...
            switch (Questions[mCurrentQuestion - 1].Type)
            {
                case QuestionType.MultipleChoice:
                    {
                        // Get the view model of a question and pass it as a parameter to new site
                        var questionViewModel = new QuestionMultipleChoiceViewModel();
                        questionViewModel.AttachQuestion(Questions[mCurrentQuestion - 1] as MultipleChoiceQuestion);
                        IoCClient.UI.ChangePage(ApplicationPage.QuestionMultipleChoice, questionViewModel);
                        break;
                    }

                case QuestionType.MultipleCheckboxes:
                    {
                        // Get the view model of a question and pass it as a parameter to new site
                        var questionViewModel = new QuestionMultipleCheckboxesViewModel();
                        questionViewModel.AttachQuestion(Questions[mCurrentQuestion - 1] as MultipleCheckboxesQuestion);
                        IoCClient.UI.ChangePage(ApplicationPage.QuestionMultipleCheckboxes, questionViewModel);
                        break;
                    }

                case QuestionType.SingleTextBox:
                    {
                        // Get the view model of a question and pass it as a parameter to new site
                        var questionViewModel = new QuestionSingleTextBoxViewModel();
                        questionViewModel.AttachQuestion(Questions[mCurrentQuestion - 1] as SingleTextBoxQuestion);
                        IoCClient.UI.ChangePage(ApplicationPage.QuestionSingleTextBox, questionViewModel);
                        break;
                    }
            }
        }

        /// <summary>
        /// Unloads the test from this host
        /// </summary>
        public void UnloadTest()
        {
            // Erase the test
            CurrentTest = null;

            // Indicate that we are out of test now
            IoCClient.Logger.Log("Test erasing");
            IsTestReceived = false;
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

            // We can iterate like this because the question list and answer list are in the same order
            for (var i = 0; i < Questions.Count; i++)
            {   
                // Based on question type...
                switch (Questions[i].Type)
                {
                    case QuestionType.MultipleChoice:
                        {
                            // Cast the answer and question objects
                            var multipleChoiceAnswer = UserAnswers[i] as MultipleChoiceAnswer;
                            var multipleChoiceQuestion = Questions[i] as MultipleChoiceQuestion;

                            var isAnswerCorrect = multipleChoiceQuestion.IsAnswerCorrect(multipleChoiceAnswer);

                            // Check if user has answered correctly
                            if (isAnswerCorrect)
                                // Give them points for this question
                                totalScore += multipleChoiceQuestion.PointScore;

                            // Create view model for the future use by the result page
                            var viewmodel = new QuestionMultipleChoiceViewModel()
                            {
                                IsAnswerCorrect = isAnswerCorrect,
                                UserAnswer = multipleChoiceAnswer,
                                IsReadOnly = true,
                                Index = i,
                            };

                            // Attach the question
                            viewmodel.AttachQuestion(multipleChoiceQuestion);

                            QuestionViewModels.Add(viewmodel);
                        }
                        break;

                    case QuestionType.MultipleCheckboxes:
                        {
                            // Cast the answer and question objects
                            var multipleCheckboxesAnswer = UserAnswers[i] as MultipleCheckboxesAnswer;
                            var multipleCheckboxesQuestion = Questions[i] as MultipleCheckboxesQuestion;

                            var isAnswerCorrect = multipleCheckboxesQuestion.IsAnswerCorrect(multipleCheckboxesAnswer);

                            // Check if user has answered correctly
                            if (isAnswerCorrect)
                                // Give them points for this question
                                totalScore += multipleCheckboxesQuestion.PointScore;

                            // Create view model for the future use by the result page
                            var viewmodel = new QuestionMultipleCheckboxesViewModel()
                            {
                                IsAnswerCorrect = isAnswerCorrect,
                                UserAnswer = multipleCheckboxesAnswer,
                                IsReadOnly = true,
                                Index = i,
                            };

                            // Attach the question
                            viewmodel.AttachQuestion(multipleCheckboxesQuestion);

                            QuestionViewModels.Add(viewmodel);
                        }
                        break;

                    case QuestionType.SingleTextBox:
                        {
                            // Cast the answer and question objects
                            var singleTextBoxAnswer = UserAnswers[i] as SingleTextBoxAnswer;
                            var singleTextBoxQuestion = Questions[i] as SingleTextBoxQuestion;

                            var isAnswerCorrect = singleTextBoxQuestion.IsAnswerCorrect(singleTextBoxAnswer);

                            // Check if user has answered correctly
                            if (isAnswerCorrect)
                                // Give them multipleCheckboxesAnswer for this question
                                totalScore += singleTextBoxQuestion.PointScore;

                            // Create view model for the future use by the result page
                            var viewmodel = new QuestionSingleTextBoxViewModel()
                            {
                                IsAnswerCorrect = isAnswerCorrect,
                                UserAnswer = singleTextBoxAnswer.Answer,
                                IsReadOnly = true,
                                Index = i,
                            };

                            // Attach the question
                            viewmodel.AttachQuestion(singleTextBoxQuestion);

                            QuestionViewModels.Add(viewmodel);
                        }
                        break;
                }
            }

            // Set calculated point score to the property and set the corresponding mark
            UserScore = totalScore;
            UserMark = CurrentTest.Grading.GetMark(UserScore);
        }

        /// <summary>
        /// Fired to indicated that connection has been re-established
        /// </summary>
        public void NetworkReconnected()
        {
            // Send update
            SendUpdate();

            // If the result has not been sent yet and the test is completed
            if (!IsResultSent && IsTestCompleted)
            {
                TrySendResult();
            }
        }

        public void NetworkDisconnected()
        {
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
                // Write results to file
                FileWriters.BinWriter.WriteToFile(new ClientTestResults()
                {
                     Answers = UserAnswers,
                     ClientModel = new ClientModelSerializable()
                     {
                         ClientName = IoCClient.Client.ClientName,
                         ClientSurname = IoCClient.Client.ClientSurname,
                         MachineName = IoCClient.Client.MachineName,
                         Mark = UserMark,
                         PointsScored = UserScore,
                     },
                     Test = CurrentTest,
                });
                
                // TODO: show message box here
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

            // Change page to the result page
            IoCClient.UI.ChangePage(ApplicationPage.ResultOverviewPage);

            // Dont need the connection in the result page so stop reconnecting if meanwhile connection has been lost
            IoCClient.Application.Network.StopReconnecting();

            // Indicate that we're in the result page
            IsShowingResultPage = true;
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
            QuestionNumber = mCurrentQuestion + " / " + Questions.Count;
        }

        /// <summary>
        /// Updates the current question number
        /// </summary>
        private void UpdateQuestionNumber()
        {
            mCurrentQuestion++;
            QuestionNumber = mCurrentQuestion + " / " + Questions.Count;
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
                TestFinished();
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using Testinator.Core;
using System.Timers;
using System.Threading;

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
        /// The test that is currently hosted
        /// </summary>
        private Test mTest = new Test();

        /// <summary>
        /// Indicates current question
        /// </summary>
        private int mCurrentQuestion = 0;

        /// <summary>
        /// List of all questions
        /// </summary>
        private List<Question> mQuestions = new List<Question>();

        /// <summary>
        /// Answers given by the user
        /// </summary>
        private List<Answer> mAnswers = new List<Answer>();

        #endregion

        #region Public Properties

        /// <summary>
        /// The test that is currently hosted
        /// </summary>
        public Test CurrentTest => mTest;

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
        /// Indicates how much time is left
        /// </summary>
        public TimeSpan TimeLeft { get; private set; }

        /// <summary>
        /// Shows which question is currently shown
        /// </summary>
        public string QuestionNumber { get; set; }

        #endregion

        #region Public Events

        /// <summary>
        /// Fired when a test is receievd
        /// </summary>
        public event Action OnTestReceived = () => { };

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts the test 
        /// </summary>
        public void Start()
        {
            // If there is no test to start or the test has already started don't do anything
            if (IsTestInProgress || !IsTestReceived)
                return;

            IsTestInProgress = true;
            ResetQuestionNumber();
            mTestTimer.Start();
            GoNextQuestion();
        }

        /// <summary>
        /// The test to be hosted
        /// </summary>
        /// <param name="test">Test to be hosted</param>
        public void BindTest(Test test)
        {
            mTest = test;
            IoCClient.Application.TimeLeft = test.Duration;
            mQuestions = test.Questions;
            TimeLeft = test.Duration;

            // Randomize questions
            mQuestions.Shuffle();

            ResetQuestionNumber();

            IsTestReceived = true;

            OnTestReceived.Invoke();
        }

        /// <summary>
        /// Stops the current test
        /// </summary>
        public void Stop()
        {
            // If there is no test to stop
            if (!IsTestInProgress)
                return;

            mTestTimer.Stop();
            IsTestInProgress = false;
            IoCClient.UI.ChangePage(ApplicationPage.ResultPage);

            // TODO: send results
        }

        /// <summary>
        /// Saves the answer for the current question
        /// </summary>
        /// <param name="answer">The answer itself</param>
        public void SaveAnswer(Answer answer)
        {
            // Make id of the question match the id of the answer
            answer.ID = mQuestions[mCurrentQuestion - 1].ID;

            // Save the answer
            mAnswers.Add(answer);
        }

        /// <summary>
        /// Goes to the next question, or
        /// Starts the test, or
        /// Shows the end screen if there is no more questions
        /// </summary>
        public void GoNextQuestion()
        {
            if (mCurrentQuestion >= mQuestions.Count)
            {
                Stop();
                return;
            }

            UpdateQuestionNumber();

            switch (mQuestions[mCurrentQuestion - 1].Type)
            {
                case QuestionType.MultipleChoice:
                    {
                        // Get the view model of a question and pass it as a parameter to new site
                        var questionViewModel = new QuestionMultipleChoiceViewModel();
                        questionViewModel.AttachQuestion(mQuestions[mCurrentQuestion - 1] as MultipleChoiceQuestion);
                        IoCClient.UI.ChangePage(ApplicationPage.QuestionMultipleChoice, questionViewModel);
                        break;
                    }

                case QuestionType.MultipleCheckboxes:
                    {
                        // Get the view model of a question and pass it as a parameter to new site
                        var questionViewModel = new QuestionMultipleCheckboxesViewModel();
                        questionViewModel.AttachQuestion(mQuestions[mCurrentQuestion - 1] as MultipleCheckboxesQuestion);
                        IoCClient.UI.ChangePage(ApplicationPage.QuestionMultipleCheckboxes, questionViewModel);
                        break;
                    }

                case QuestionType.SingleTextBox:
                    {
                        // Get the view model of a question and pass it as a parameter to new site
                        var questionViewModel = new QuestionSingleTextBoxViewModel();
                        questionViewModel.AttachQuestion(mQuestions[mCurrentQuestion - 1] as SingleTextBoxQuestion);
                        IoCClient.UI.ChangePage(ApplicationPage.QuestionSingleTextBox, questionViewModel);
                        break;
                    }
            }
            SendUpdate();

        }

        #endregion

        #region Private Helpers

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
                    QuestionSolved = mCurrentQuestion,
                },
            };
            
            // Send it to the server
            IoCClient.Application.Network.SendData(data);
        }

        /// <summary>
        /// Resets the question number
        /// </summary>
        private void ResetQuestionNumber()
        {
            mCurrentQuestion = 0;
            QuestionNumber = mCurrentQuestion + " / " + mQuestions.Count;
        }

        /// <summary>
        /// Updates the current question number
        /// </summary>
        private void UpdateQuestionNumber()
        {
            mCurrentQuestion++;
            QuestionNumber = mCurrentQuestion + " / " + mQuestions.Count;
        }

        /// <summary>
        /// Handles the cutdown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleTimer(object sender, ElapsedEventArgs e)
        {
            TimeLeft = TimeLeft.Subtract(new TimeSpan(0, 0, 1));
            if (TimeLeft.Equals(new TimeSpan(0, 0, 0)))
            {
                Stop();
            }
        }

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
    }
}

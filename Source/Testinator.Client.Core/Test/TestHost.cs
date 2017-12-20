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

        private Timer TestTimer = new Timer(1000);

        /// <summary>
        /// The test that is currently hosted
        /// </summary>
        private Test _Test = new Test();

        /// <summary>
        /// Indicates current question
        /// </summary>
        private int CurrentQuestion = 0;

        /// <summary>
        /// List of all questions
        /// </summary>
        private List<Question> Questions = new List<Question>();

        /// <summary>
        /// Answers given by the user
        /// </summary>
        private List<Answer> Answers = new List<Answer>();

        #endregion

        #region Public Properties

        /// <summary>
        /// The test that is currently hosted
        /// </summary>
        public Test CurrentTest => _Test;

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
            TestTimer.Start();
            GoNextQuestion();
        }

        /// <summary>
        /// The test to be hosted
        /// </summary>
        /// <param name="test">Test to be hosted</param>
        public void BindTest(Test test)
        {
            _Test = test;
            IoCClient.Application.TimeLeft = test.Duration;
            Questions = test.Questions;
            TimeLeft = test.Duration;

            // Randomize questions
            Questions.Shuffle();

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

            TestTimer.Stop();
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
            answer.ID = Questions[CurrentQuestion - 1].ID;

            // Save the answer
            Answers.Add(answer);
        }

        /// <summary>
        /// Goes to the next question, or
        /// Starts the test, or
        /// Shows the end screen if there is no more questions
        /// </summary>
        public void GoNextQuestion()
        {
            if (CurrentQuestion >= Questions.Count)
            {
                Stop();
                return;
            }

            switch (Questions[CurrentQuestion].Type)
            {
                case QuestionType.MultipleChoice:
                    {
                        // Get the view model of a question and pass it as a parameter to new site
                        var questionViewModel = new QuestionMultipleChoiceViewModel();
                        questionViewModel.AttachQuestion(Questions[CurrentQuestion] as MultipleChoiceQuestion);
                        IoCClient.Application.GoToPage(ApplicationPage.QuestionMultipleChoice, questionViewModel);
                        break;
                    }

                case QuestionType.MultipleCheckboxes:
                    {
                        // Get the view model of a question and pass it as a parameter to new site
                        var questionViewModel = new QuestionMultipleCheckboxesViewModel();
                        questionViewModel.AttachQuestion(Questions[CurrentQuestion] as MultipleCheckboxesQuestion);
                        IoCClient.Application.GoToPage(ApplicationPage.QuestionMultipleCheckboxes, questionViewModel);
                        break;
                    }

                case QuestionType.SingleTextBox:
                    {
                        // Get the view model of a question and pass it as a parameter to new site
                        var questionViewModel = new QuestionSingleTextBoxViewModel();
                        questionViewModel.AttachQuestion(Questions[CurrentQuestion] as SingleTextBoxQuestion);
                        IoCClient.Application.GoToPage(ApplicationPage.QuestionSingleTextBox, questionViewModel);
                        break;
                    }
            }

            UpdateQuestionNumber();

        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Resets the question number
        /// </summary>
        private void ResetQuestionNumber()
        {
            CurrentQuestion = 1;
            IoCClient.Application.QuestionNumber = CurrentQuestion + " / " + Questions.Count;
        }

        /// <summary>
        /// Updates the current question number
        /// </summary>
        private void UpdateQuestionNumber()
        {
            CurrentQuestion++;
            IoCClient.Application.QuestionNumber = CurrentQuestion + " / " + Questions.Count;
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
            TestTimer.Elapsed += HandleTimer;
        }

        #endregion
    }
}

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

        #endregion

        #region Public Properties

        /// <summary>
        /// The test that is currently hosted
        /// </summary>
        public Test CurrentTest { get; set; } = new Test();

        /// <summary>
        /// Indicates current question user is facing
        /// </summary>
        public int CurrentQuestion { get; private set; } = 0;

        /// <summary>
        /// List of all questions in the test
        /// </summary>
        public List<Question> Questions { get; private set; }

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
        /// Indicates how much time is left
        /// </summary>
        public TimeSpan TimeLeft { get; private set; }

        /// <summary>
        /// The user's score
        /// </summary>
        public int UserScore { get; private set; }

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
            // If there is no test to start or the test has already started don't do anything
            if (!IsTestReceived || IsTestInProgress)
                return;

            // Indicate that test is starting
            IsTestInProgress = true;

            // Reset question number, so user starts from first question
            UpdateQuestionNumber(true);

            // Initialize the answer list so user can add his answer to it
            UserAnswers = new List<Answer>();

            // Start the test timer
            mTestTimer.Start();

            // Show first question
            GoNextQuestion();
        }

        /// <summary>
        /// Binds the test to this view model 
        /// </summary>
        /// <param name="test">Test to be hosted</param>
        public void BindTest(Test test)
        {
            // Get the test and save it in this view model
            CurrentTest = test;
            Questions = test.Questions;
            TimeLeft = test.Duration;

            // Randomize question order
            Questions.Shuffle();

            // Indicate that we have received test
            IsTestReceived = true;
            OnTestReceived.Invoke();
        }

        /// <summary>
        /// Stops the current test
        /// </summary>
        public void StopTest()
        {
            // If there is no test to stop, just return
            if (!IsTestInProgress)
                return;

            // Stop the timer, we don't need it anymore
            mTestTimer.Stop();

            // Indicate that test has ended
            IsTestInProgress = false;

            // Change page to result page
            IoCClient.UI.ChangePage(ApplicationPage.ResultOverviewPage);
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
            UserAnswers.Add(answer);
        }

        /// <summary>
        /// Goes to the next question, or
        /// Starts the test, or
        /// Shows the end screen if there is no more questions
        /// </summary>
        public void GoNextQuestion()
        {
            // If last question was the last question, finish the test
            if (CurrentQuestion >= Questions.Count)
            {
                TestFinished();
                return;
            }

            // Update the question number
            UpdateQuestionNumber(false);

            // Send to the server that client has passed the previous question
            SendUpdate();

            // Based on next question type...
            switch (Questions[CurrentQuestion - 1].Type)
            {
                case QuestionType.MultipleChoice:
                    {
                        // Get the view model of a question and pass it as a parameter to new site
                        var questionViewModel = new QuestionMultipleChoiceViewModel();
                        questionViewModel.AttachQuestion(Questions[CurrentQuestion - 1] as MultipleChoiceQuestion);
                        IoCClient.UI.ChangePage(ApplicationPage.QuestionMultipleChoice, questionViewModel);
                        break;
                    }

                case QuestionType.MultipleCheckboxes:
                    {
                        // Get the view model of a question and pass it as a parameter to new site
                        var questionViewModel = new QuestionMultipleCheckboxesViewModel();
                        questionViewModel.AttachQuestion(Questions[CurrentQuestion - 1] as MultipleCheckboxesQuestion);
                        IoCClient.UI.ChangePage(ApplicationPage.QuestionMultipleCheckboxes, questionViewModel);
                        break;
                    }

                case QuestionType.SingleTextBox:
                    {
                        // Get the view model of a question and pass it as a parameter to new site
                        var questionViewModel = new QuestionSingleTextBoxViewModel();
                        questionViewModel.AttachQuestion(Questions[CurrentQuestion - 1] as SingleTextBoxQuestion);
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
            CurrentTest = new Test();

            // Indicate that we are out of test now
            IsTestReceived = false;
        }

        /// <summary>
        /// Compares the both answers and questions lists and calculates user point score based on that
        /// </summary>
        public void CompareUserAnswersAndCalculatePoints()
        {
            // Keep track of user total point score
            var totalScore = 0;

            // For each question...
            foreach (var question in Questions)
            {
                // Look for the match answer
                foreach (var answer in UserAnswers)
                    if (answer.ID == question.ID)
                    {
                        // Based on question type...
                        switch (question.Type)
                        {
                            case QuestionType.MultipleChoice:
                                {
                                    // Cast the answer and question objects
                                    var multipleChoiceAnswer = answer as MultipleChoiceAnswer;
                                    var multipleChoiceQuestion = question as MultipleChoiceQuestion;

                                    // Check if user has answered correctly
                                    if (multipleChoiceQuestion.CorrectAnswerIndex == multipleChoiceAnswer.SelectedAnswerIdx)
                                        // Give him points for this question
                                        totalScore += multipleChoiceQuestion.PointScore;
                                }
                                break;

                            case QuestionType.MultipleCheckboxes:
                                {
                                    // Cast the answer and question objects
                                    var multipleCheckboxesAnswer = answer as MultipleCheckboxesAnswer;
                                    var multipleCheckboxesQuestion = question as MultipleCheckboxesQuestion;

                                    // TODO: Advanced checkboxes logic (giving points for every good answer, or only for the whole set etc.)
                                }
                                break;

                            case QuestionType.SingleTextBox:
                                {
                                    // Cast the answer and question objects
                                    var singleTextBoxAnswer = answer as SingleTextBoxAnswer;
                                    var singleTextBoxQuestion = question as SingleTextBoxQuestion;

                                    // Catch the right answer string and user answer string
                                    var rightAnswer = singleTextBoxAnswer.Answer;
                                    var userAnswer = singleTextBoxQuestion.CorrectAnswer;

                                    // Remove white spaces from them
                                    rightAnswer = rightAnswer.Trim();
                                    userAnswer = userAnswer.Trim();

                                    // TODO: Check in settings if it the answer has to be case sensitive
                                    if (!false)//isCaseSensitivityImportant?
                                    {
                                        // Answer doesn't have to be case sensitive, make every string as lowercase
                                        rightAnswer = rightAnswer.ToLower();
                                        userAnswer = userAnswer.ToLower();
                                    }

                                    // Check if answer matches
                                    if (rightAnswer == userAnswer)
                                        // Give user points for this question
                                        totalScore += singleTextBoxQuestion.PointScore;
                                }
                                break;
                        }

                        // We have handled answer for this question, go to the next one
                        break;
                    }
            }

            // Set calculated point score to the property
            UserScore = totalScore;
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Fired when the current test is finished
        /// </summary>
        private void TestFinished()
        {

            
            StopTest();
        }

        private void CalculateScore()
        { 

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
                    QuestionSolved = CurrentQuestion,
                },
            };
            
            // Send it to the server
            IoCClient.Application.Network.SendData(data);
        }

        /// <summary>
        /// Updates the current question number
        /// Or resets it if requested
        /// </summary>
        private void UpdateQuestionNumber(bool reset)
        {
            // If sender wants to reset the counter, set it to 0
            if (reset) CurrentQuestion = 0;

            // Otherwise, increment it
            else CurrentQuestion++;

            // Update the question number display in the view
            IoCClient.Application.QuestionNumber = CurrentQuestion + " / " + Questions.Count;
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
                StopTest();
        }

        #endregion
    }
}

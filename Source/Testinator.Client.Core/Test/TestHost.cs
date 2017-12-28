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
<<<<<<< HEAD
        /// Shows which question is currently shown
        /// </summary>
        public string QuestionNumber { get; set; }
=======
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
>>>>>>> ResultPage

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
        public void CalculateScore()
        {
            // Total point score
            var totalScore = 0;

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

                            // Attach the question and the correct answer flag
                            viewmodel.AttachQuestion(multipleChoiceQuestion);

                            QuestionViewModels.Add(viewmodel);
                        }
                        break;

                    case QuestionType.MultipleCheckboxes:
                        {
                            // Cast the answer and question objects
                            var multipleCheckboxesAnswer = UserAnswers[i] as MultipleCheckboxesAnswer;
                            var multipleCheckboxesQuestion = Questions[i] as MultipleCheckboxesQuestion;

                            var isAnswerCorrect = multipleCheckboxesQuestion.IsAnswerCorrect(multipleCheckboxesQuestion);

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

                            // Attach the question and the correct answer flag
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

                            // Attach the question and the correct answer flag
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

        #endregion

        #region Private Helpers

        /// <summary>
        /// Fired when the current test is finished
        /// </summary>
        private void TestFinished()
        {
            CalculateScore();
            
            StopTest();
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
<<<<<<< HEAD
        /// Resets the question number
        /// </summary>
        private void ResetQuestionNumber()
        {
            mCurrentQuestion = 0;
            QuestionNumber = mCurrentQuestion + " / " + mQuestions.Count;
        }

        /// <summary>
=======
>>>>>>> ResultPage
        /// Updates the current question number
        /// Or resets it if requested
        /// </summary>
        private void UpdateQuestionNumber(bool reset)
        {
<<<<<<< HEAD
            mCurrentQuestion++;
            QuestionNumber = mCurrentQuestion + " / " + mQuestions.Count;
=======
            // If sender wants to reset the counter, set it to 0
            if (reset) CurrentQuestion = 0;

            // Otherwise, increment it
            else CurrentQuestion++;

            // Update the question number display in the view
            IoCClient.Application.QuestionNumber = CurrentQuestion + " / " + Questions.Count;
>>>>>>> ResultPage
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

using System;
using System.Collections.Generic;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Client.Core
{
    /// <summary>
    /// The view model for test result page 
    /// </summary>
    public class ResultViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The original test received from server
        /// </summary>
        public Test Test { get; private set; }

        /// <summary>
        /// List of questions in the test user has accomplished
        /// </summary>
        public List<Question> Questions { get; private set; }

        /// <summary>
        /// List of answers user has sent
        /// </summary>
        public List<Answer> UserAnswers { get; private set; }

        /// <summary>
        /// The time in which user has completed the test
        /// </summary>
        public TimeSpan CompletionTime => Test.Duration - IoCClient.Application.TimeLeft;

        /// <summary>
        /// The score user achieved
        /// </summary>
        public int UserScore { get; set; }

        /// <summary>
        /// The mark user has achieved by doing the test
        /// </summary>
        public Marks UserMark => Test.Grading.GetMark(UserScore);

        #endregion

        #region Commands

        /// <summary>
        /// The command to exit the result page and go back to waiting for test page
        /// </summary>
        public ICommand ExitCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ResultViewModel()
        {
            // Load data from TestHost
            LoadTestHostData();

            // Calculate user's score
            UserScore = CompareUserAnswersAndCalculatePoints();

            // Create commands
            ExitCommand = new RelayCommand(Exit);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Cleans the previous test and goes back to the waiting for test page
        /// </summary>
        private void Exit()
        {
            // Clean previous test from this application
            IoCClient.TestHost.UnloadTest();

            // Go to the waiting for test page
            IoCClient.Application.GoToPage(ApplicationPage.WaitingForTest);
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Loads data from TestHost to this view model
        /// </summary>
        private void LoadTestHostData()
        {
            // Get the original test
            Test = IoCClient.TestHost.CurrentTest;

            // Get the list of questions
            Questions = IoCClient.TestHost.Questions;

            // Get user answers
            UserAnswers = IoCClient.TestHost.UserAnswers;
        }

        /// <summary>
        /// Compares the both answers and questions lists and calculates user point score based on that
        /// </summary>
        /// <returns></returns>
        private int CompareUserAnswersAndCalculatePoints()
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

            // Return calculated point score
            return totalScore;
        }

        #endregion
    }
}

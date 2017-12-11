using System;
using System.Collections.Generic;
using System.Linq;
using Testinator.Core;

namespace Testinator.Client.Core
{
    /// <summary>
    /// Responsible for hosting a test
    /// </summary>
    public class TestHost
    {
        #region Private Members

        /// <summary>
        /// The test that is currently hosted
        /// </summary>
        private Test mTest;

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

        #region Public Methods

        /// <summary>
        /// The test to be hosted
        /// </summary>
        /// <param name="test">Test to be hosted</param>
        public void BindTest(Test test)
        {
            mTest = test;
            IoCClient.Application.TimeLeft = test.Duration;
            Questions = test.Questions;

            // Randomize questions
            Questions.Shuffle();

            CurrentQuestion = 1;

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
                // TODO: change it to the result page
                IoCClient.Application.GoToPage(ApplicationPage.WaitingForTest);
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

        private void UpdateQuestionNumber()
        {
            CurrentQuestion++;
            IoCClient.Application.QuestionNumber = CurrentQuestion + " / " + Questions.Count;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestHost()
        { }

        #endregion
    }
}

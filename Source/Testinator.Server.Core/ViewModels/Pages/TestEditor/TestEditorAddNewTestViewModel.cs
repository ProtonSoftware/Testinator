using System;
using System.Collections.Generic;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The view model for adding new test in TestEditor page
    /// </summary>
    public class TestEditorAddNewTestViewModel : BaseViewModel
    {
        #region Private Members

        // Displayed in combobox menu
        private string mMultipleChoiceTranslatedString = "Wielokrotny wybór (Jedna odpowiedź)";
        private string mMultipleCheckboxesTranslatedString = "Wielokrotny wybór (Wiele odpowiedzi)";
        private string mSingleTextBoxTranslatedString = "Wpisywana odpowiedź";

        #endregion

        #region Public Properties

        /// <summary>
        /// The name of the test user wants to create
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// The time this test is going to take to complete (as string)
        /// </summary>
        public string Duration { get; set; } = "";

        /// <summary>
        /// The test itself, builded step by step
        /// </summary>
        public Test Test { get; set; }

        /// <summary>
        /// A flag indicating if invalid data error should be shown
        /// </summary>
        public bool InvalidDataError { get; set; } = false;

        /// <summary>
        /// Question type as string from combobox
        /// </summary>
        public string QuestionBeingAddedString { get; set; } = "";

        /// <summary>
        /// The type of the question being added right now
        /// </summary>
        public QuestionType QuestionBeingAddedType
        {
            get
            {
                string windowsPrefix = "System.Windows.Controls.ComboBoxItem: ";

                // Based on what is chosen in combobox...
                if (QuestionBeingAddedString == windowsPrefix + mMultipleChoiceTranslatedString) return QuestionType.MultipleChoice;
                if (QuestionBeingAddedString == windowsPrefix + mMultipleCheckboxesTranslatedString) return QuestionType.MultipleCheckboxes;
                if (QuestionBeingAddedString == windowsPrefix + mSingleTextBoxTranslatedString) return QuestionType.SingleTextBox;

                // None found, return default value
                return QuestionType.None;
            }
        }

        #region Multiple Choice Answers

        public string AnswerA { get; set; }
        public string AnswerB { get; set; }
        public string AnswerC { get; set; }
        public string AnswerD { get; set; }
        public string AnswerE { get; set; }

        public int HowManyMultipleChoiceAnswersVisible = 2;

        public bool ShouldAnswerCBeVisible { get; set; } = false;
        public bool ShouldAnswerDBeVisible { get; set; } = false;
        public bool ShouldAnswerEBeVisible { get; set; } = false;

        #endregion

        #region Multiple Checkboxes Answers

        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }
        public string Answer5 { get; set; }

        public int HowManyMultipleCheckboxesAnswersVisible = 2;

        public bool ShouldAnswer3BeVisible { get; set; } = false;
        public bool ShouldAnswer4BeVisible { get; set; } = false;
        public bool ShouldAnswer5BeVisible { get; set; } = false;

        #endregion

        #endregion

        #region Commands

        /// <summary>
        /// The command to change page to question adding page
        /// </summary>
        public ICommand AddingQuestionsPageChangeCommand { get; private set; }

        /// <summary>
        /// The command to submit newly created question and add it to the test
        /// </summary>
        public ICommand SubmitQuestionCommand { get; private set; }

        /// <summary>
        /// The command to add new answer to the question
        /// </summary>
        public ICommand AddAnswerCommand { get; private set; }

        /// <summary>
        /// The command to remove an additional answer from the question
        /// </summary>
        public ICommand RemoveAnswerCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestEditorAddNewTestViewModel()
        {
            // Create commands
            AddingQuestionsPageChangeCommand = new RelayCommand(ChangePage);
            SubmitQuestionCommand = new RelayCommand(SubmitQuestion);
            AddAnswerCommand = new RelayCommand(AddAnswer);
            RemoveAnswerCommand = new RelayCommand(RemoveAnswer);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Changes page to question adding page
        /// </summary>
        private void ChangePage()
        {
            // Disable previous errors
            InvalidDataError = false;

            // Check if input data is valid
            if (this.Name.Length < 3 || !Int32.TryParse(Duration, out int time) || time > 500 || time <= 0)
            {
                // Show error and dont change page
                InvalidDataError = true;
                return;
            }

            // Save this view model
            var viewModel = new TestEditorAddNewTestViewModel();
            viewModel.Name = this.Name;
            viewModel.Duration = this.Duration;
            viewModel.Test = this.Test;

            // Pass it to the next page
            IoCServer.Application.GoToPage(ApplicationPage.TestEditorAddQuestions, viewModel);
        }

        /// <summary>
        /// Adds an answer to the question
        /// </summary>
        private void AddAnswer()
        {
            // Check which question type is being created
            if (QuestionBeingAddedType == QuestionType.MultipleChoice)
            {
                // Show next answer
                HowManyMultipleChoiceAnswersVisible++;
                switch (HowManyMultipleChoiceAnswersVisible)
                {
                    case 3:
                        ShouldAnswerCBeVisible = true;
                        break;
                    case 4:
                        ShouldAnswerDBeVisible = true;
                        break;
                    case 5:
                        ShouldAnswerEBeVisible = true;
                        break;
                }
}
            else if (QuestionBeingAddedType == QuestionType.MultipleCheckboxes)
            {
                // Show next answer
                HowManyMultipleCheckboxesAnswersVisible++;
                switch (HowManyMultipleCheckboxesAnswersVisible)
                {
                    case 3:
                        ShouldAnswer3BeVisible = true;
                        break;
                    case 4:
                        ShouldAnswer4BeVisible = true;
                        break;
                    case 5:
                        ShouldAnswer5BeVisible = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Removes an answer from the question
        /// </summary>
        private void RemoveAnswer()
        {
            // Check which question type is being created
            if (QuestionBeingAddedType == QuestionType.MultipleChoice)
            {
                // Remove answer based on counter
                switch (HowManyMultipleChoiceAnswersVisible)
                {
                    case 5:
                        ShouldAnswerEBeVisible = false;
                        break;
                    case 4:
                        ShouldAnswerDBeVisible = false;
                        break;
                    case 3:
                        ShouldAnswerCBeVisible = false;
                        break;
                }

                // Decrement the counter
                HowManyMultipleChoiceAnswersVisible--;
            }
            else if (QuestionBeingAddedType == QuestionType.MultipleCheckboxes)
            {
                // Remove answer based on counter
                switch (HowManyMultipleCheckboxesAnswersVisible)
                {
                    case 5:
                        ShouldAnswer5BeVisible = false;
                        break;
                    case 4:
                        ShouldAnswer4BeVisible = false;
                        break;
                    case 3:
                        ShouldAnswer3BeVisible = false;
                        break;
                }

                // Decrement the counter
                HowManyMultipleCheckboxesAnswersVisible--;
            }
        }

        /// <summary>
        /// Submits newly created question and adds it to the test
        /// </summary>
        private void SubmitQuestion()
        {
            return;
        }

        #endregion
    }
}

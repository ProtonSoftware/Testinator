using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        /// List of current questions in test converted to an observable collection
        /// </summary>
        public ObservableCollection<Question> Questions { get; set; } = new ObservableCollection<Question>();

        /// <summary>
        /// A flag indicating if invalid data error should be shown
        /// </summary>
        public bool InvalidDataError { get; set; } = false;

        /// <summary>
        /// A flag indicating if test menu should be expanded
        /// </summary>
        public bool IsTestMenuExpanded { get; set; } = true;

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

        /// <summary>
        /// The task of the question
        /// </summary>
        public string QuestionTask { get; set; } = "";

        #region Multiple Choice Answers

        public string AnswerA { get; set; }
        public string AnswerB { get; set; }
        public string AnswerC { get; set; }
        public string AnswerD { get; set; }
        public string AnswerE { get; set; }

        public int HowManyMultipleChoiceAnswersVisible = 2;
        public string QuestionMultipleChoicePointScore { get; set; }
        public string RightAnswerIdx { get; set; } = "1";

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

        public bool IsAnswer1Right { get; set; }
        public bool IsAnswer2Right { get; set; }
        public bool IsAnswer3Right { get; set; }
        public bool IsAnswer4Right { get; set; }
        public bool IsAnswer5Right { get; set; }

        public int HowManyMultipleCheckboxesAnswersVisible = 2;
        public string QuestionMultipleCheckboxesPointScore { get; set; }

        public bool ShouldAnswer3BeVisible { get; set; } = false;
        public bool ShouldAnswer4BeVisible { get; set; } = false;
        public bool ShouldAnswer5BeVisible { get; set; } = false;

        #endregion

        #region Single TextBox Answers

        public string TextBoxAnswer { get; set; }
        public string QuestionSingleTextBoxPointScore { get; set; }

        #endregion

        #endregion

        #region Commands

        /// <summary>
        /// The command to change page to question adding page
        /// </summary>
        public ICommand AddingQuestionsPageChangeCommand { get; private set; }

        /// <summary>
        /// The command to change page to criteria adding page
        /// </summary>
        public ICommand AddingCriteriaPageChangeCommand { get; private set; }

        /// <summary>
        /// The command to expand/hide the test menu
        /// </summary>
        public ICommand TestMenuExpandCommand { get; private set; }

        /// <summary>
        /// The command to submit newly created question and add it to the test
        /// </summary>
        public ICommand SubmitQuestionCommand { get; private set; }

        /// <summary>
        /// The command to submit newly created test
        /// </summary>
        public ICommand SubmitTestCommand { get; private set; }

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
            AddingQuestionsPageChangeCommand = new RelayCommand(ChangeQuestionsPage);
            AddingCriteriaPageChangeCommand = new RelayCommand(ChangeCriteriaPage);
            TestMenuExpandCommand = new RelayCommand(ExpandMenu);
            SubmitQuestionCommand = new RelayCommand(SubmitQuestion);
            SubmitTestCommand = new RelayCommand(SubmitTest);
            AddAnswerCommand = new RelayCommand(AddAnswer);
            RemoveAnswerCommand = new RelayCommand(RemoveAnswer);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Changes page to question adding page
        /// </summary>
        private void ChangeQuestionsPage()
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

            // Create new test and assign data to it
            Test = new Test();
            Test.Name = this.Name;
            Test.Duration = new TimeSpan(0, time, 0);

            // Save the view model and change page
            SaveViewModelAndChangeToNewQuestion();
        }

        /// <summary>
        /// Changes page to question adding page
        /// </summary>
        private void ChangeCriteriaPage()
        {
            // Check if test has at least one question
            if (Test.Questions.Count < 1)
                // TODO: Error handling
                return;

            // Save this view model
            var viewModel = new TestEditorAddNewTestViewModel();
            viewModel.Test = this.Test;

            // Pass it to the next page
            IoCServer.Application.GoToPage(ApplicationPage.TestEditorAddCriteria, viewModel);
        }

        /// <summary>
        /// Expands/hides the test menu
        /// </summary>
        private void ExpandMenu()
        {
            // Simply toggle the expanded flag
            IsTestMenuExpanded ^= true;
        }

        /// <summary>
        /// Saves the current state of view model and changes page to the add new question page
        /// </summary>
        private void SaveViewModelAndChangeToNewQuestion()
        {
            // Save this view model
            var viewModel = new TestEditorAddNewTestViewModel();
            viewModel.Test = this.Test;
            foreach (var question in Test.Questions) viewModel.Questions.Add(question);

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
            // Based on type
            switch(QuestionBeingAddedType)
            {
                case QuestionType.MultipleChoice:
                    {       //// TODO: Error handling!!!
                        // Create and build a question based on values
                        var question = new MultipleChoiceQuestion();
                        question.Task = this.QuestionTask;
                        if (!Int32.TryParse(this.QuestionMultipleChoicePointScore, out int pointScore))
                        {
                            // Wrong value in textbox input, show error
                            return;
                        }
                        question.PointScore = pointScore;
                        question.Options = new List<string>();
                        question.Options.Add(AnswerA);
                        question.Options.Add(AnswerB);
                        if (ShouldAnswerCBeVisible) question.Options.Add(AnswerC);
                        if (ShouldAnswerDBeVisible) question.Options.Add(AnswerD);
                        if (ShouldAnswerEBeVisible) question.Options.Add(AnswerE);
                        if (!Int32.TryParse(this.RightAnswerIdx, out int rightAnswerIdx))
                        {
                            // Wrong value in textbox input, show error
                            return;
                        }
                        question.CorrectAnswerIndex = rightAnswerIdx;

                        // We have our question done, add it to the test
                        Test.AddQuestion(question);

                        // Go to adding next question
                        SaveViewModelAndChangeToNewQuestion();
                    }
                    break;

                case QuestionType.MultipleCheckboxes:
                    {       //// TODO: Error handling!!!
                        // Create and build a question based on values
                        var question = new MultipleCheckboxesQuestion();
                        question.Task = this.QuestionTask;
                        question.OptionsAndAnswers = new Dictionary<string, bool>();
                        question.OptionsAndAnswers.Add(Answer1, IsAnswer1Right);
                        question.OptionsAndAnswers.Add(Answer2, IsAnswer2Right);
                        if (ShouldAnswer3BeVisible) question.OptionsAndAnswers.Add(Answer3, IsAnswer3Right);
                        if (ShouldAnswer4BeVisible) question.OptionsAndAnswers.Add(Answer4, IsAnswer4Right);
                        if (ShouldAnswer5BeVisible) question.OptionsAndAnswers.Add(Answer5, IsAnswer5Right);
                        if (!Int32.TryParse(this.QuestionMultipleCheckboxesPointScore, out int pointScore))
                        {
                            // Wrong value in textbox input, show error
                            return;
                        }
                        question.PointScore = pointScore;

                        // We have our question done, add it to the test
                        Test.AddQuestion(question);

                        // Go to adding next question
                        SaveViewModelAndChangeToNewQuestion();
                    }
                    break;

                case QuestionType.SingleTextBox:
                    {       //// TODO: Error handling!!!
                        // Create and build a question based on values
                        var question = new SingleTextBoxQuestion();
                        question.Task = this.QuestionTask;
                        question.CorrectAnswer = this.TextBoxAnswer;
                        if (!Int32.TryParse(this.QuestionSingleTextBoxPointScore, out int pointScore))
                        {
                            // Wrong value in textbox input, show error
                            return;
                        }
                        question.PointScore = pointScore;

                        // We have our question done, add it to the test
                        Test.AddQuestion(question);

                        // Go to adding next question
                        SaveViewModelAndChangeToNewQuestion();
                    }
                    break;

                default:
                    // Question type not found, don't submit any question
                    return;
            }
        }

        /// <summary>
        /// Finally submits the whole test we've created
        /// </summary>
        private void SubmitTest()
        {
            // TODO: Save it to binary file, add to list etc.
            return;
        }

        #endregion
    }
}

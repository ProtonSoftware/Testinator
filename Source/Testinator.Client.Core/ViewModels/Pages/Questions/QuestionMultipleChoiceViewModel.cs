using System;
using System.Collections.Generic;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Client.Core
{
    /// <summary>
    /// The view model for <see cref="MultipleChoiceQuestion"/>
    /// </summary>
    public class QuestionMultipleChoiceViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// The question this view model is based on
        /// </summary>
        private MultipleChoiceQuestion mQuestion = new MultipleChoiceQuestion();

        #endregion

        #region Public Properties

        /// <summary>
        /// The task of the question
        /// </summary>
        public string Task => mQuestion.Task;

        /// <summary>
        /// Options for the questions to choose from eg. A, B, C...
        /// </summary>
        public List<ABCAnswerItemViewModel> Options { get; set; }

        /// <summary>
        /// Index of answer currently selected
        /// IMPORTANT: 1 means that the first option in the list is seleced, 2 that second, etc.
        /// 0 means that no answer is selected
        /// </summary>
        public int CurrentlySelectedIdx { get; set; } = 0;

        /// <summary>
        /// Number of options available
        /// </summary>
        public int Count => Options.Count;

        /// <summary>
        /// Sets the visibility of the no answer warning
        /// When user chooses the answer automatically is set to false
        /// </summary>
        public bool NoAnswerWarning { get; set; } = false;

        /// <summary>
        /// Points gained for the correct answer
        /// </summary>
        public int PointScore => mQuestion.PointScore;

        #endregion

        #region Commands

        /// <summary>
        /// Submits the current question and procceds to the next question
        /// </summary>
        public ICommand SubmitCommand { get; set; }

        /// <summary>
        /// Makes the answer selected,
        /// NOTE: command parameter MUST be the answer index, look at <see cref="CurrentlySelecedIdx"/>
        /// </summary>
        public ICommand SelectCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default construcotr
        /// </summary>
        public QuestionMultipleChoiceViewModel()
        {
            // Create commands
            SubmitCommand = new RelayCommand(Submit);
            SelectCommand = new RelayParameterizedCommand(Select);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Select the answer
        /// </summary>
        /// <param name="obj">The index of the answer, also look at <see cref="CurrentlySelecedIdx"/>/></param>
        private void Select(object obj)
        {
            // Cast parameter
            var idxStr = obj as string;
            if (!Int32.TryParse(idxStr, out int idx))
                throw new NotImplementedException();

            CurrentlySelectedIdx = idx;

            NoAnswerWarning = false;
        }

        /// <summary>
        /// Submits the current question
        /// </summary>
        private void Submit()
        {
            if (CurrentlySelectedIdx == 0)
            {
                NoAnswerWarning = true;
                return;
            }

            // Save the answer
            var answer = new MultipleChoiceAnswer(mQuestion, CurrentlySelectedIdx);
            IoCClient.Application.Test.AddAnswer(answer);

            // Go to next page
            var question = IoCClient.Application.Test.GetNextQuestion();

            // Based on type...
            switch (question.Type)
            {
                case QuestionTypes.MultipleChoice:
                    {
                        // Get the view model of a question and pass it as a parameter to new site
                        var questionViewModel = new QuestionMultipleChoiceViewModel();
                        questionViewModel.AttachQuestion(question as MultipleChoiceQuestion);
                        IoCClient.Application.GoToPage(ApplicationPage.QuestionMultipleChoice, questionViewModel);
                        break;
                    }

                case QuestionTypes.MultipleCheckboxes:
                    {
                        // Get the view model of a question and pass it as a parameter to new site
                        var questionViewModel = new QuestionMultipleCheckboxesViewModel();
                        questionViewModel.AttachQuestion(question as MultipleCheckboxesQuestion);
                        IoCClient.Application.GoToPage(ApplicationPage.QuestionMultipleCheckboxes, questionViewModel);
                        break;
                    }

                case QuestionTypes.SingleTextBox:
                    {
                        // Get the view model of a question and pass it as a parameter to new site
                        var questionViewModel = new QuestionSingleTextBoxViewModel();
                        questionViewModel.AttachQuestion(question as SingleTextBoxQuestion);
                        IoCClient.Application.GoToPage(ApplicationPage.QuestionSingleTextBox, questionViewModel);
                        break;
                    }
            }
        }

        #endregion

        #region Public Helpers

        /// <summary>
        /// Adds question this view model will be based on
        /// NOTE: needs to be done before attaching this view model to the page
        /// </summary>
        /// <param name="question">The question to be attached to this viewmodel</param>
        public void AttachQuestion(MultipleChoiceQuestion question)
        {
            // Get the question
            mQuestion = question;

            // Convert the list of string to list of ABCAnswerItemViewModel
            Options = ListConvertFromStringQuestion(mQuestion.Options);
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Takes in a list of strings and converts it to actual list of answer items
        /// </summary>
        /// <param name="options">Possible answers to the question as list of string</param>
        /// <returns></returns>
        private List<ABCAnswerItemViewModel> ListConvertFromStringQuestion(List<string> options)
        {
            // Initialize the list that we are willing to return 
            var FinalList = new List<ABCAnswerItemViewModel>();

            // Loop each answer to create answer item from it
            int answerCounter = 1;
            foreach (var option in options)
            {
                // Create new answer item
                var answerItem = new ABCAnswerItemViewModel();

                // Attach proper letter to it
                switch(answerCounter)
                {
                    case 1: answerItem.Letter = "A"; break;
                    case 2: answerItem.Letter = "B"; break;
                    case 3: answerItem.Letter = "C"; break;
                    case 4: answerItem.Letter = "D"; break;
                    case 5: answerItem.Letter = "E"; break;
                }

                // Rewrite answer string content
                answerItem.Text = option;

                // Don't select any answer at the start
                answerItem.IsSelected = false;

                // Add this item to the list
                FinalList.Add(answerItem);

                // Go to the next answer
                answerCounter++;
            }

            // We have our list done, return it
            return FinalList;
        }

        #endregion
    }
}

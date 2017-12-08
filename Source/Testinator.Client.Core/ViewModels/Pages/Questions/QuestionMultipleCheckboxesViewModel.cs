using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Client.Core
{
    /// <summary>
    /// A viewmodel for <see cref="MultipleCheckboxesQuestion"/>
    /// </summary>
    public class QuestionMultipleCheckboxesViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// The question this view model is based on
        /// </summary>
        private MultipleCheckboxesQuestion mQuestion;

        #endregion

        #region Public Properties

        /// <summary>
        /// The task of the question
        /// </summary>
        public string Task => mQuestion.Task;

        /// <summary>
        /// Options for the questions to check or uncheck
        /// </summary>
        public List<string> Options => mQuestion.OptionList();

        /// <summary>
        /// Indexs of answer currently checked and unchecked
        /// false means unchecked, true means unchecked
        /// </summary>
        public ObservableCollection<bool> CurrentlyChecked { get; set; }

        /// <summary>
        /// Number of options available
        /// </summary>
        public int Count => Options.Count;

        /// <summary>
        /// Sets the visibility of the no-answer warning
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
        /// Check or unchecks the option
        /// NOTE: command parameter MUST be the answer index, INDEXING STARTS AT 1!
        /// </summary>
        public ICommand SelectCommand { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds question this view model will be based on
        /// NOTE: needs to be done before attaching this view model to the page
        /// </summary>
        /// <param name="question">The question to be attached to this viewmodel</param>
        public void AttachQuestion(MultipleCheckboxesQuestion question)
        {
            mQuestion = question;

            CurrentlyChecked = new ObservableCollection<bool>();

            // Make all the answers unchecked
            for (int i = 0; i < mQuestion.OptionList().Count; i++)
                CurrentlyChecked.Add(false);
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public QuestionMultipleCheckboxesViewModel()
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

            // idx - 1 because indexing starts at 1 not 0 !
            CurrentlyChecked[idx - 1] = true;

            NoAnswerWarning = false;
        }

        /// <summary>
        /// Submits the current question
        /// </summary>
        private void Submit()
        {
            // If none of the options is checked show the warning to the user
            if (!CurrentlyChecked.Contains(true))
            {
                NoAnswerWarning = true;
                return;
            }
            var answer = new MultipleCheckboxesAnswer(mQuestion, new List<bool>(CurrentlyChecked));
            // TODO: save the answer and show the next question
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// A viewmodel for <see cref="MultipleChoiceQuestion"/>
    /// </summary>
    public class MultipleChoiceQuestionViewModel : BaseViewModel
    {
        #region Private Properties

        /// <summary>
        /// The question this view model is based on
        /// </summary>
        private MultipleChoiceQuestion mQuestion { get; set; }

        #endregion

        #region Public Properties

        /// <summary>
        /// The task of the question
        /// </summary>
        public string Task => mQuestion.Task;

        /// <summary>
        /// Options for the questions to choose from eg. A, B, C...
        /// </summary>
        public List<string> Options => mQuestion.Options;

        /// <summary>
        /// Index of answer currently selected
        /// IMPORTANT: 1 means that the first option in the list is seleced, 2 that second, etc.
        /// 0 means that no answer is selected
        /// </summary>
        public int CurrentlySelecedIdx { get; set; } = 0;

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
        /// Creates view model from the given question
        /// </summary>
        /// <param name="question">The question this view model will be based on</param>
        public MultipleChoiceQuestionViewModel(MultipleChoiceQuestion question)
        {
            // Create commands
            SubmitCommand = new RelayCommand(Submit);
            SelectCommand = new RelayParameterizedCommand(Select);

            // Save the question
            mQuestion = question;
        }

        #endregion

        #region Public Methods

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

            CurrentlySelecedIdx = idx;

            NoAnswerWarning = false;
        }

        /// <summary>
        /// Submits the current question
        /// </summary>
        private void Submit()
        {
            if (CurrentlySelecedIdx == 0)
            {
                NoAnswerWarning = true;
                return;
            }

            var answer = new MultipleChoiceAnswer(mQuestion, CurrentlySelecedIdx);
            // TODO: save the answer and show the next question
        }

        #endregion
    }
}

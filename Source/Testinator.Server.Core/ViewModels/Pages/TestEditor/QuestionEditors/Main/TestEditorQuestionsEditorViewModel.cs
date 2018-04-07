using System;
using System.Collections.Generic;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The view model for editing questions phase
    /// </summary>
    public class TestEditorQuestionsEditorViewModel : BaseViewModel
    {
        #region Commands

        /// <summary>
        /// Submits current question
        /// </summary>
        public ICommand SubmitCommand { get; private set; }

        /// <summary>
        /// Cancels current question edition/creating
        /// </summary>
        public ICommand CancelCommand { get; private set; }

        /// <summary>
        /// Goes to the next creation phase
        /// </summary>
        public ICommand GoNextPhaseCommand { get; private set; }

        #endregion

        #region Command Methods

        /// <summary>
        /// Submits the current question
        /// </summary>
        private void Submit()
        {

        }

        /// <summary>
        /// Go to the next phase of creation/edition of the test
        /// </summary>
        private void GoNextPhase()
        {

        }

        /// <summary>
        /// Cancels current question edition/creating
        /// </summary>
        private void Cancel()
        {
            
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestEditorQuestionsEditorViewModel()
        {
            QuestionListViewModel.Instance.LoadItems(null);
            CreateCommands();
        }

        /// <summary>
        /// Initializes this question editor with the given data
        /// </summary>
        /// <param name="Questions"></param>
        public TestEditorQuestionsEditorViewModel(List<Question> Questions)
        {
            Questions.Add(new MultipleChoiceQuestion()
            {
                CorrectAnswerIndex = 0,
                Options = new List<string>() { "one", "two" },
                Scoring = new Scoring(ScoringMode.FullAnswer, 10),
                Task = new TaskContent("Some long question task so it is hard to display it in one line. rly.")
            });
            Questions.Add(new MultipleChoiceQuestion()
            {
                CorrectAnswerIndex = 0,
                Options = new List<string>() { "one", "two" },
                Scoring = new Scoring(ScoringMode.FullAnswer, 10),
                Task = new TaskContent("Some222222 long question task so it is hard to display it in one line. rly.")
            });
            QuestionListViewModel.Instance.LoadItems(Questions);
            CreateCommands();
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// Creates command for this viewmodel
        /// </summary>
        private void CreateCommands()
        {
            SubmitCommand = new RelayCommand(Submit);
            CancelCommand = new RelayCommand(Cancel);
            GoNextPhaseCommand = new RelayCommand(GoNextPhase);
        }

        #endregion
    }
}

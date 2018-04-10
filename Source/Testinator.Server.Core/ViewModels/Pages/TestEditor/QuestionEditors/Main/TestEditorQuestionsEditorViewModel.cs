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
        #region Public Properties

        /// <summary>
        /// Indicates if the dialog with question type choice is visible
        /// </summary>
        public bool QuestionTypeDialogVisible { get; set; } = true;

        /// <summary>
        /// The viewmodel for images editor
        /// </summary>
        public ImagesEditorViewModel ImagesEditorViewModel { get; private set; }

        /// <summary>
        /// The view model for the currently edited question
        /// </summary>
        public BaseQuestionEditorViewModel QuestionViewModel { get; private set; }

        /// <summary>
        /// Current question type being edited right noew
        /// </summary>
        public QuestionType CurrentQuestionType { get; set; }
        
        #endregion

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

        /// <summary>
        /// The command to select question type from the list
        /// </summary>
        public ICommand SelectQuestionTypeCommand { get; private set; }

        #endregion

        #region Command Methods

        /// <summary>
        /// Submits the current question
        /// </summary>
        private void Submit()
        {
            QuestionTypeDialogVisible ^= true;
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

        /// <summary>
        /// Select question type from the popup
        /// </summary>
        /// <param name="Type">The type of the question as <see cref="QuestionType"/> enum</param>
        private void SelectQuestion(object Type)
        {
            try
            {
                var type = (QuestionType)Type;
                CurrentQuestionType = type;
            }
            catch
            {
                // Developer error
                IoCServer.Logger.Log("No such question. Error code: 1");
                return;
            }

            QuestionTypeDialogVisible = false;
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
            ImagesEditorViewModel = new ImagesEditorViewModel();
        }

        /// <summary>
        /// Initializes this question editor with the given data
        /// </summary>
        /// <param name="Questions"></param>
        public TestEditorQuestionsEditorViewModel(List<Question> Questions)
        {
            ImagesEditorViewModel = new ImagesEditorViewModel();
            ImagesEditorViewModel.PropertyChanged += (s, e) => OnPropertyChanged(nameof(ImagesEditorViewModel));
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
            SelectQuestionTypeCommand = new RelayParameterizedCommand(SelectQuestion);
        }

        #endregion
    }
}

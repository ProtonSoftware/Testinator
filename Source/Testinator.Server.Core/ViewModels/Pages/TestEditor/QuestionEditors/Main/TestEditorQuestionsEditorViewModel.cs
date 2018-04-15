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
        /// The view model for the currently edited question
        /// </summary>
        public BaseQuestionEditorViewModel CurrentQuestionEditorViewModel { get; private set; }

        /// <summary>
        /// Current question type being edited right noew
        /// </summary>
        public QuestionType CurrentQuestionType { get; set; }

        /// <summary>
        /// The current number of questions in test
        /// </summary>
        public int CurrentQuestionsCount => IoCServer.TestEditor.Builder.CurrentQuestions.Count;

        /// <summary>
        /// The current total score of this test
        /// </summary>
        public int CurrentTotalPointsScore => IoCServer.TestEditor.Builder.CurrentPointScore;

        /// <summary>
        /// Indicates if cancel button should be visible
        /// </summary>
        public bool IsCancelButtonVisible { get; private set; }

        /// <summary>
        /// Indicates if the user can delete current question
        /// </summary>
        public bool CanDeleteQuestion { get; private set; }

        #endregion

        #region Commands

        /// <summary>
        /// Submits current question
        /// </summary>
        public ICommand SubmitCommand { get; private set; }

        /// <summary>
        /// Cancels current question edition
        /// </summary>
        public ICommand CancelCommand { get; private set; }

        /// <summary>
        /// The command to delete current question from the test
        /// </summary>
        public ICommand DeleteQuestionCommand { get; private set; }

        /// <summary>
        /// Goes to the next creation phase
        /// </summary>
        public ICommand GoNextPhaseCommand { get; private set; }

        /// <summary>
        /// The command to select question type from the list
        /// </summary>
        public ICommand SelectQuestionTypeCommand { get; private set; }

        /// <summary>
        /// The command to go back to the previous page
        /// </summary>
        public ICommand GoPreviousPageCommand { get; private set; }
        
        #endregion

        #region Command Methods

        /// <summary>
        /// Fired when submit button is clicked
        /// </summary>
        private void Submit()
        {
            if (SubmitQuestion())
            {
                QuestionTypeDialogVisible = true;
                QuestionListViewModel.Instance.UnCheckAll();
                CurrentQuestionEditorViewModel = BaseQuestionEditorViewModel.ToViewModel(null);
            }
        }

        /// <summary>
        /// Go to the next phase of creation/edition of the test
        /// </summary>
        private void GoNextPhase()
        {
            if (CurrentQuestionEditorViewModel != null && CurrentQuestionEditorViewModel.AnyUnsavedChanges)
            {
                if (AskToSave())
                {
                    if (!SubmitQuestion())
                        return;
                }

                QuestionTypeDialogVisible = true;
                CurrentQuestionEditorViewModel = null;
            }
            
            if (IoCServer.TestEditor.Builder.CurrentQuestions.Count == 0)
            {
                IoCServer.UI.ShowMessage(new MessageBoxDialogViewModel()
                {
                    Message = "Nie można przejść dalej, ponieważ test nie posiada pytań!",
                    Title = "Test editor",
                });

                return;
            }

            if (IoCServer.TestEditor.Builder.CurrentPointScore <= 4)
            {
                IoCServer.UI.ShowMessage(new MessageBoxDialogViewModel()
                {
                    Message = "Nie można przejść dalej, ponieważ maksymalna liczba punktów za test jest mniejsza od 5!",
                    Title = "Test editor",
                });

                return;
            }

            IoCServer.TestEditor.GoNextPhase();
        }

        /// <summary>
        /// Goes to the previous page
        /// </summary>
        private void GoPreviousPage()
        {
            if (CurrentQuestionEditorViewModel != null && CurrentQuestionEditorViewModel.AnyUnsavedChanges)
            {
                if (AskToSave())
                {
                    if (!SubmitQuestion())
                        return;
                }
            }

            IoCServer.TestEditor.GoPreviousPage();
        }

        /// <summary>
        /// Deletes current question from the test
        /// </summary>
        private void DeleteQuestion()
        {
            IoCServer.TestEditor.DeleteQuestion(CurrentQuestionEditorViewModel.OriginalQuestion);
            QuestionListViewModel.Instance.UnCheckAll();
            QuestionListViewModel.Instance.CanChangeSelection = true;
            QuestionTypeDialogVisible = true;
        }

        /// <summary>
        /// Cancels current question edition/creating
        /// </summary>
        private void Cancel()
        {
            if (CurrentQuestionEditorViewModel.AnyUnsavedChanges)
            {
                if (AskToSave())
                {
                    if (!SubmitQuestion())
                        return;
                }
                else
                {
                    QuestionListViewModel.Instance.CanChangeSelection = true;
                    QuestionListViewModel.Instance.SelectLastClickedItem();
                }
            }

            ImagesEditorViewModel.Instance.LoadItems(null);
            CurrentQuestionEditorViewModel = BaseQuestionEditorViewModel.ToViewModel(null);
            QuestionListViewModel.Instance.UnCheckAll();
            QuestionTypeDialogVisible = true;
        }

        /// <summary>
        /// Select question type from the popup
        /// </summary>
        /// <param name="Type">The type of the question as <see cref="QuestionType"/> enum</param>
        private void SelectQuestionFromTiles(object Type)
        {
            try
            {
                var type = (QuestionType)Type;
                CurrentQuestionType = type;
                IsCancelButtonVisible = false;
                CanDeleteQuestion = false;
                CurrentQuestionEditorViewModel = BaseQuestionEditorViewModel.ToViewModel(null, type);
            }
            catch
            {
                // Developer error
                IoCServer.Logger.Log("No such question. Error code: 1");
                return;
            }
            
            QuestionTypeDialogVisible = false;
        }

        /// <summary>
        /// Fired when a question is selected from the side menu
        /// </summary>
        /// <param name="SelectedQuestion">The question that has been selected</param>
        private void QuestionListItemSelectedEvent(int SelectedQuestionIndex)
        {
            if (CurrentQuestionEditorViewModel != null && CurrentQuestionEditorViewModel.AnyUnsavedChanges)
            {
                if (AskToSave())
                {
                    if (SubmitQuestion())
                    {
                        QuestionListViewModel.Instance.CanChangeSelection = true;
                        QuestionListViewModel.Instance.SelectLastClickedItem();
                    }
                    else
                        return;
                }
                else
                {
                    QuestionListViewModel.Instance.CanChangeSelection = true;
                    QuestionListViewModel.Instance.SelectLastClickedItem();
                }
            }

            var SelectedQuestion = IoCServer.TestEditor.Builder.CurrentQuestions[SelectedQuestionIndex];

            CurrentQuestionEditorViewModel = BaseQuestionEditorViewModel.ToViewModel(SelectedQuestion);
            CurrentQuestionType = SelectedQuestion.Type;
            ImagesEditorViewModel.Instance.LoadItems(SelectedQuestion.Task.Images);
            QuestionTypeDialogVisible = false;
            IsCancelButtonVisible = true;
            CanDeleteQuestion = true;
            
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestEditorQuestionsEditorViewModel()
        {
            QuestionListViewModel.Instance.LoadItems(null);
            Initialize();
            CurrentQuestionEditorViewModel = null;
        }

        /// <summary>
        /// Initializes this question editor with the given data
        /// </summary>
        /// <param name="Questions"></param>
        public TestEditorQuestionsEditorViewModel(List<Question> Questions)
        {
            QuestionListViewModel.Instance.LoadItems(Questions);
            Initialize();
        }
        
        #endregion

        #region Private Helpers

        /// <summary>
        /// Asks the user if they want to save changes to the question or not
        /// </summary>
        /// <returns>True if they want to; otherwise, false</returns>
        private bool AskToSave()
        {
            var vm = new DecisionDialogViewModel()
            {
                Message = "Czy chcesz zapisać zmiany w obecnym pytaniu?",
                AcceptText = "Tak",
                CancelText = "Nie",
                Title = "Edytor testów",
            };

            IoCServer.UI.ShowMessage(vm);

            return vm.UserResponse;
        }

        /// <summary>
        /// Submits current question
        /// </summary>
        /// <returns>True if operation succeed; othwerwise, false</returns>
        private bool SubmitQuestion()
        {
            var NewQuestion = CurrentQuestionEditorViewModel.Submit();
            
            if (NewQuestion != null)
            {
                NewQuestion.Task.AddImages(ImagesEditorViewModel.Instance.Images);

                // Clear images control
                ImagesEditorViewModel.Instance.LoadItems(null);

                // Update question
                if (CurrentQuestionEditorViewModel.IsInEditMode)
                {
                    IoCServer.TestEditor.Builder.UpdateQuestion(CurrentQuestionEditorViewModel.OriginalQuestion, NewQuestion);
                    QuestionListViewModel.Instance.UpdateQuestion(CurrentQuestionEditorViewModel.OriginalQuestion, NewQuestion);
                }
                // Add question
                else
                {
                    IoCServer.TestEditor.Builder.AddQuestion(NewQuestion);
                    QuestionListViewModel.Instance.AppendQuestion(NewQuestion);
                }

                IoCServer.TestEditor.UpdateQuestion();

                return true;
            }
            else
                return false;
            
        }

        /// <summary>
        /// Creates command for this viewmodel
        /// </summary>
        private void CreateCommands()
        {
            SubmitCommand = new RelayCommand(Submit);
            CancelCommand = new RelayCommand(Cancel);
            GoNextPhaseCommand = new RelayCommand(GoNextPhase);
            SelectQuestionTypeCommand = new RelayParameterizedCommand(SelectQuestionFromTiles);
            DeleteQuestionCommand = new RelayCommand(DeleteQuestion);
            GoPreviousPageCommand = new RelayCommand(GoPreviousPage);
        }

        /// <summary>
        /// Updates properties responsible for showing information about question numbers and full point score
        /// </summary>
        private void UpdateQuestionsInformation()
        {
            OnPropertyChanged(nameof(CurrentQuestionsCount));
            OnPropertyChanged(nameof(CurrentTotalPointsScore));
        }

        /// <summary>
        /// Fired when selection in question list changes
        /// </summary>
        private void QuestionListViewModel_SelectionChanges()
        {
            // Keep track of any selection changes in questions control to work properly with unsaved changes
            if (CurrentQuestionEditorViewModel != null)
            {
                if (CurrentQuestionEditorViewModel.AnyUnsavedChanges)
                    QuestionListViewModel.Instance.CanChangeSelection = false;
            }
        }

        /// <summary>
        /// Initialies this viewmodel,
        /// As it has to constructors we would have to copy-pase the code
        /// </summary>
        private void Initialize()
        {
            CreateCommands();

            ImagesEditorViewModel.Instance.LoadItems(null);

            // Fired when data about a test changes, so we can update properties
            IoCServer.TestEditor.QuestionsChanged += UpdateQuestionsInformation;

            QuestionListViewModel.Instance.ItemSelected += QuestionListItemSelectedEvent;

            // Keep track of any selection changes in questions control to work properly with unsaved changes
            QuestionListViewModel.Instance.SelectionChanges += QuestionListViewModel_SelectionChanges;
        }

        /// <summary>
        /// Disposes this viewmodel
        /// </summary>
        public override void Dispose()
        {
            // Unsubscribe from all events
            QuestionListViewModel.Instance.ItemSelected -= QuestionListItemSelectedEvent;
            IoCServer.TestEditor.QuestionsChanged -= UpdateQuestionsInformation;
            QuestionListViewModel.Instance.ItemSelected -= QuestionListItemSelectedEvent;
            QuestionListViewModel.Instance.SelectionChanges -= QuestionListViewModel_SelectionChanges;

            if(CurrentQuestionEditorViewModel != null)
                CurrentQuestionEditorViewModel.Dispose();
        }

        #endregion
    }
}

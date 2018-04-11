﻿using System;
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
        /// Fired when submit button is clicked
        /// </summary>
        private void Submit()
        {
            if (SubmitQuestion())
                QuestionTypeDialogVisible = true;
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
                CurrentQuestionEditorViewModel = BaseQuestionEditorViewModel.ToViewModel(type);
            }
            catch
            {
                // Developer error
                IoCServer.Logger.Log("No such question. Error code: 1");
            }
            finally
            {
                QuestionTypeDialogVisible = false;
            }
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

        #region Private Methods

        /// <summary>
        /// Fired when a question is selected from the side menu
        /// </summary>
        /// <param name="SelectedQuestion">The question that has been selected</param>
        private void QuestionListItemSelectedEvent(int SelectedQuestionIndex)
        {
            if (CurrentQuestionEditorViewModel.AnyUnsavedChanges)
            {
                var vm = new DecisionDialogViewModel()
                {
                    Message = "Czy chcesz zapisać zmiany w obecnym pytaniu?",
                    AcceptText = "Tak",
                    CancelText = "Nie",
                    Title = "Zmiana pytania",
                };

                if(vm.UserResponse)
                {
                    SubmitQuestion();
                }
            }

            var SelectedQuestion = IoCServer.TestEditor.Builder.CurrentQuestions[SelectedQuestionIndex];

            CurrentQuestionEditorViewModel = BaseQuestionEditorViewModel.ToViewModel(SelectedQuestion.Type);
            CurrentQuestionEditorViewModel.AttachQuestion(SelectedQuestion);
            CurrentQuestionType = SelectedQuestion.Type;

            ImagesEditorViewModel.LoadItems(SelectedQuestion.Task.Images);
            QuestionTypeDialogVisible = false;

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
                NewQuestion.Task.AddImages(ImagesEditorViewModel.Images);

                // Clear images control
                ImagesEditorViewModel.LoadItems(null);

                if (CurrentQuestionEditorViewModel.IsInEditMode)
                {
                    IoCServer.TestEditor.Builder.UpdateQuestion(CurrentQuestionEditorViewModel.OriginalQuestion, NewQuestion);
                    QuestionListViewModel.Instance.UpdateQuestion(CurrentQuestionEditorViewModel.OriginalQuestion, NewQuestion);
                }
                else
                {
                    IoCServer.TestEditor.Builder.AddQuestion(NewQuestion);
                    QuestionListViewModel.Instance.AppendQuestion(NewQuestion);
                }

                QuestionListViewModel.Instance.UnCheckAll();
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
            SelectQuestionTypeCommand = new RelayParameterizedCommand(SelectQuestion);
        }

        /// <summary>
        /// Initialies this viewmodel,
        /// As it has to constructors we would have to copy-pase the code
        /// </summary>
        private void Initialize()
        {
            CreateCommands();
            ImagesEditorViewModel = new ImagesEditorViewModel();

            // Fired when data about a test changes, so we can update properties
            IoCServer.TestEditor.QuestionsChanged += () =>
            {
                OnPropertyChanged(nameof(CurrentQuestionsCount));
                OnPropertyChanged(nameof(CurrentTotalPointsScore));
            };

            QuestionListViewModel.Instance.ItemSelected += QuestionListItemSelectedEvent;
        }

        #endregion
    }
}
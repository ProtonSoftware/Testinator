using System;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// Handles creation/edition process of any test
    /// </summary>
    public class TestEditor : BaseViewModel
    {
        #region Private Members

        

        #endregion

        #region Public Properties

        /// <summary>
        /// The test builder this test editor uses to edit or create tests
        /// </summary>
        public TestBuilder Builder { get; private set; }

        /// <summary>
        /// Indicates if there are some changes done to the test
        /// </summary>
        public bool AnyUnsavedChanges { get; private set; }

        /// <summary>
        /// Current Operation that the test editor is doing
        /// </summary>
        public Operation CurrentOperation { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads a test for editing
        /// </summary>
        /// <param name="TestToEdit"></param>
        public void EditTest(Test TestToEdit)
        {
            // Load the builder
            Builder = new TestBuilder(TestToEdit);
            AnyUnsavedChanges = false;
        }

        /// <summary>
        /// Deleletes a question form the test
        /// </summary>
        /// <param name="QuestionToDelete">The question to delet</param>
        public void DeleteQuestion(Question QuestionToDelete)
        {
            Builder.RemoveQuestion(QuestionToDelete);
            QuestionListViewModel.Instance.RemoveQuestion(QuestionToDelete);
            QuestionsChanged.Invoke();
        }

        /// <summary>
        /// Fires the question update event to update the view
        /// </summary>
        public void UpdateQuestion()
        {
            QuestionsChanged.Invoke();
        }

        /// <summary>
        /// Initializes editor to create a new test
        /// </summary>
        public void CreateNewTest()
        {
            Builder = new TestBuilder();
            AnyUnsavedChanges = true;
        }

        /// <summary>
        /// Start the process of either editing or creating a test
        /// </summary>
        public void Start()
        {
            // Thats the starting page
            GoToInformationEditor();
        }

        /// <summary>
        /// Notifies the editor that there was some change in question/information or whatever that has been confirmed
        /// Used in case user wants to close without saving
        /// </summary>
        public void TestChanged() => AnyUnsavedChanges = true;

        /// <summary>
        /// Goes to the next stage of test creation/edition
        /// </summary>
        public void GoNextPhase()
        {
            switch (CurrentOperation)
            {
                case Operation.EditingInformation:
                    GoToQuestionsEditor();
                    break;

                case Operation.EditingQuestions:
                    GoToCriteriaEditor();
                    break;

                case Operation.EditingCriteria:
                    GoToFinalizing();
                    break;

                case Operation.Finalizing:
                    Finish();
                    break;
            }
        }

        /// <summary>
        /// Exits from the editor
        /// </summary>
        public void Exit()
        {
            if (AnyUnsavedChanges)
            {
                var vm = new DecisionDialogViewModel()
                {
                    Message = "There are some unsaved changes. Do you want to exit without saving?",
                    AcceptText = "Yes",
                    CancelText = "No",
                    Title = "Exit",
                };
                IoCServer.UI.ShowMessage(vm);
                if (vm.UserResponse)
                    IoCServer.Application.GoToPage(ApplicationPage.TestEditorInitial);
            }
        }

        /// <summary>
        /// Returns to the previous screen
        /// </summary>
        public void Return()
        {
            switch (CurrentOperation)
            {
                case Operation.EditingInformation:

                    // That one is treated like an exit
                    Exit();
                    break;

                case Operation.EditingQuestions:
                    GoToInformationEditor();
                    break;

                case Operation.EditingCriteria:
                    GoToQuestionsEditor();
                    break;

                case Operation.Finalizing:
                    GoToQuestionsEditor();
                    break;
            }
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Fired when questions change, so any viewmodels can update their properties
        /// </summary>
        public event Action QuestionsChanged = () => { };

        #endregion

        #region Private Methods

        /// <summary>
        /// Goes to the information editor page
        /// </summary>
        private void GoToInformationEditor()
        {
            IoCServer.Application.GoToPage(ApplicationPage.TestEditorBasicInformationEditor, 
                Builder.CurrentTestInfo == null ? new TestEditorBasicInformationEditorViewModel(Builder.CurrentTestInfo) : null);
            CurrentOperation = Operation.EditingInformation;
        }

        /// <summary>
        /// Goes to the questions editor page
        /// </summary>
        private void GoToQuestionsEditor()
        {
            IoCServer.Application.GoToPage(ApplicationPage.TestEditorQuestionsEditor, new TestEditorQuestionsEditorViewModel(Builder.CurrentQuestions));
            CurrentOperation = Operation.EditingQuestions;
        }

        /// <summary>
        /// Goes to the criteria editor page
        /// </summary>
        private void GoToCriteriaEditor()
        {
            CurrentOperation = Operation.EditingCriteria;
        }

        /// <summary>
        /// Goes to the criteria editor page
        /// </summary>
        private void GoToFinalizing()
        {
            CurrentOperation = Operation.EditingCriteria;
        }

        /// <summary>
        /// Finishes the work of this edtior by saving the test
        /// </summary>
        private void Finish()
        {
            CurrentOperation = Operation.EditingCriteria;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestEditor()
        {
            Builder = new TestBuilder();
        }

        #endregion

    }
}

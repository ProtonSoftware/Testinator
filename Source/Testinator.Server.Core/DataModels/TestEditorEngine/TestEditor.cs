using System;
using System.Collections.Generic;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// Handles creation/edition process of any test
    /// </summary>
    public class TestEditor : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// Writer for test
        /// </summary>
        private BinaryWriter mTestFileWriter = new BinaryWriter(SaveableObjects.Test);
        
        #endregion

        #region Public Properties

        /// <summary>
        /// The test builder this test editor uses to edit or create tests
        /// </summary>
        public TestBuilder Builder { get; private set; }

        /// <summary>
        /// Indicates if the editor is in edit mode (it's working on an exisiting test)
        /// </summary>
        public bool IsInEditMode { get; private set; }

        /// <summary>
        /// Original object being edited, makes sense if <see cref="IsInEditMode"/> is set
        /// </summary>
        public Test OriginalTest { get; private set; }

        /// <summary>
        /// Indicates if there are some changes done to the test
        /// </summary>
        public bool AnyUnsavedChanges { get; private set; }

        /// <summary>
        /// Current Operation that the test editor is doing
        /// </summary>
        public Operation CurrentOperation { get; private set; }

        /// <summary>
        /// The name of currently edited/created test
        /// </summary>
        public string CurrentTestName => Builder == null ? "Nan" : Builder.CurrentTestName;

        /// <summary>
        /// Current grading for this test
        /// </summary>
        public GradingPoints CurrentGrading => Builder?.CurrentGrading;

        /// <summary>
        /// Current duration of the test
        /// </summary>
        public string CurrentDuration => Builder == null ? "Nan" : Builder.CurrentDuration;

        /// <summary>
        /// Current tags associated with this test
        /// </summary>
        public string CurrentTags => Builder == null ? "Nan" : Builder.CurrentTags;

        /// <summary>
        /// Current full point score for this test
        /// </summary>
        public int CurrentPointScore => Builder == null ? 0 : Builder.CurrentPointScore;

        /// <summary>
        /// Current question counts based on the question type
        /// </summary>
        public Dictionary<QuestionType, int> CurrentQuestionsNumber => Builder?.CurrentQuestionsNumber;

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
            IsInEditMode = true;
            OriginalTest = TestToEdit;
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
        public void UpdateQuestion() => QuestionsChanged.Invoke();

        /// <summary>
        /// Initializes editor to create a new test
        /// </summary>
        public void CreateNewTest()
        {
            Builder = new TestBuilder();
            AnyUnsavedChanges = true;
            IsInEditMode = false;
            OriginalTest = null;
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
        public void GoPreviousPage()
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
                    GoToCriteriaEditor();
                    break;
            }
        }

        /// <summary>
        /// Saves the test to file 
        /// </summary>
        /// <returns>True if successful; otherwise, false</returns>
        /// <param name="FileName">File name for the test, if not specified default name is used</param>
        public bool Save(string FileName = "")
        {
            if (IsInEditMode)
                FileName = "";

            var Test = IoCServer.TestEditor.Builder.GetResult();

            Test.Info.SoftwareVersion = IoCServer.Application.Version;

            try
            {
                mTestFileWriter.WriteToFile(Test, FileName);
            }
            catch (Exception ex)
            {
                IoCServer.UI.ShowMessage(new MessageBoxDialogViewModel()
                {
                    Title = "Test editor",
                    Message = $"Nie można zapisać pliku. Treść błędu: {ex.Message}",
                    OkText = "Ok",
                });

                return false;
            }

            return true;          
        }

        /// <summary>
        /// Finishes the work and goes back to the main page
        /// </summary>
        public void FinishAndClose()
        {
            CleanUp();
            IoCServer.Application.GoToPage(ApplicationPage.TestEditorInitial);
        }

        /// <summary>
        /// Gets the file name associated with the current test object
        /// </summary>
        /// <returns></returns>
        public string GetCurrentTestFileName()
        {
            if (OriginalTest == null)
                return "NaN";

            if (BinaryReader.AllTests.ContainsKey(OriginalTest))
                return BinaryReader.AllTests[OriginalTest];

            return "Nan";
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
            IoCServer.Application.GoToPage(ApplicationPage.TestEditorBasicInformationEditor, new TestEditorBasicInformationEditorViewModel(Builder.CurrentTestInfo));
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
            IoCServer.Application.GoToPage(ApplicationPage.TestEditorAttachCriteria, new TestEditorAttachCriteriaViewModel());
            CurrentOperation = Operation.EditingCriteria;
        }

        /// <summary>
        /// Goes to the criteria editor page
        /// </summary>
        private void GoToFinalizing()
        {
            IoCServer.Application.GoToPage(ApplicationPage.TestEditorFinalize, new TestEditorFinalizingViewModel());
            CurrentOperation = Operation.Finalizing;
        }

        /// <summary>
        /// Cleans up 
        /// </summary>
        private void CleanUp()
        {
            Builder = null;
            CurrentOperation = Operation.None;
            OriginalTest = null;
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

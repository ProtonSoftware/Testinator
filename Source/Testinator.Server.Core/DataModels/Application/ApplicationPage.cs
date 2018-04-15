namespace Testinator.Server.Core
{
    /// <summary>
    /// Every page in this application as an enum
    /// </summary>
    public enum ApplicationPage
    {
        /// <summary>
        /// No page
        /// </summary>
        None = 0,

        /// <summary>
        /// The initial login page
        /// </summary>
        Login,

        /// <summary>
        /// The home page shown after logging in
        /// </summary>
        Home,

        /// <summary>
        /// The page to begin the whole test system
        /// </summary>
        BeginTest,

        /// <summary>
        /// The subpage of BeginTestPage for starting a test
        /// </summary>
        BeginTestInitial,

        /// <summary>
        /// The subpage for showing list of saved tests
        /// </summary>
        BeginTestChoose,

        /// <summary>
        /// The subpage for showing info about choosen test
        /// </summary>
        BeginTestInfo,

        /// <summary>
        /// The subpage for showing test in progress status
        /// </summary>
        BeginTestInProgress,

        /// <summary>
        /// The subpage for showing the results
        /// </summary>
        BeginTestResults,

        /// <summary>
        /// The test editor initial page
        /// </summary>
        TestEditorInitial,

        /// <summary>
        /// The page to create new criteria for future tests
        /// </summary>
        TestEditorCriteriaEditor,

        /// <summary>
        /// The page for editing basic information about a test
        /// </summary>
        TestEditorBasicInformationEditor,

        /// <summary>
        /// The page to edit questions
        /// </summary>
        TestEditorQuestionsEditor,

        /// <summary>
        /// The page to manage tests. Delete or edit them or something else
        /// </summary>
        TestEditorTestManagmentPage,

        /// <summary>
        /// The page to attach criteria to previously created test
        /// </summary>
        TestEditorAttachCriteria,

        /// <summary>
        /// The page to finalize test creation process
        /// </summary>
        TestEditorFinalize,

        /// <summary>
        /// The test results initial page
        /// </summary>
        TestResultsInitial,

        /// <summary>
        /// The test results details page
        /// </summary>
        TestResultsDetails,

        /// <summary>
        /// The page that shows results based on students
        /// </summary>
        TestResultsStudentsView,

        /// <summary>
        /// The page tat shows results based on questions
        /// </summary>
        TestResultsQuestionsView,

        /// <summary>
        /// The page that show additional options for result page
        /// </summary>
        TestResultsDetailsView,

        /// <summary>
        /// The page to show multiple checkboxes question [used in result page]
        /// </summary>
        QuestionMultipleCheckboxes,

        /// <summary>
        /// The page to show multiple choice question [used in result page] 
        /// </summary>
        QuestionMultipleChoice,

        /// <summary>
        /// The page to show single text box question [used in result page] 
        /// </summary>
        QuestionSingleTextBox,

        /// <summary>
        /// The host page for showing questions
        /// </summary>
        ResultQuestions,

        /// <summary>
        /// The screen stream page
        /// </summary>
        ScreenStream,

        /// <summary>
        /// The settings page
        /// </summary>
        Settings,

        /// <summary>
        /// Info about the application
        /// </summary>
        About
    }
}

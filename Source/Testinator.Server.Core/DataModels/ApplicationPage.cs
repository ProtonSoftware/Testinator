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
        /// The page to create/edit tests
        /// </summary>
        TestEditor,

        /// <summary>
        /// The page to create new criteria for future tests
        /// </summary>
        TestEditorAddNewCriteria,

        /// <summary>
        /// The page to create new test
        /// </summary>
        TestEditorAddTest,

        /// <summary>
        /// The page to add questions to previously created test on TestEditorAddTest page
        /// </summary>
        TestEditorAddQuestions,

        /// <summary>
        /// The page to attach criteria to previously created test
        /// </summary>
        TestEditorAttachCriteria,

        /// <summary>
        /// The page for showing that test is created and proceed successfully
        /// </summary>
        TestEditorResult,

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

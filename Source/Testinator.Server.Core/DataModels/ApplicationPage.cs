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
        Login = 1,

        /// <summary>
        /// The home page shown after logging in
        /// </summary>
        Home = 2,

        /// <summary>
        /// The page to begin the whole test system
        /// </summary>
        BeginTest = 3,

        /// <summary>
        /// The page to create/edit tests
        /// </summary>
        TestEditor = 4,

        /// <summary>
        /// The page to create new test
        /// </summary>
        TestEditorAddTest = 5,

        /// <summary>
        /// The page to add questions to previously created test on TestEditorAddTest page
        /// </summary>
        TestEditorAddQuestions = 6,

        /// <summary>
        /// The screen stream page
        /// </summary>
        ScreenStream = 7,

        /// <summary>
        /// The settings page
        /// </summary>
        Settings = 8,

        /// <summary>
        /// Info about the application
        /// </summary>
        About = 9
    }
}

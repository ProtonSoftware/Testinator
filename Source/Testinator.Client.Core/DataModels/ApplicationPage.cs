namespace Testinator.Client.Core
{
    /// <summary>
    /// Every page in this application as an enum
    /// </summary>
    public enum ApplicationPage
    {
        /// <summary>
        /// No page
        /// </summary>
        None,

        /// <summary>
        /// The initial login page
        /// </summary>
        Login,

        /// <summary>
        /// The page which waits for the test from server app
        /// </summary>
        WaitingForTest,

        /// <summary>
        /// The page that contains test results
        /// </summary>
        ResultPage,
        
        #region Question Pages

        /// <summary>
        /// The multiple choice question page (ABCD)
        /// </summary>
        QuestionMultipleChoice,

        /// <summary>
        /// The multiple checkboxes question page (option checked or not)
        /// </summary>
        QuestionMultipleCheckboxes,

        /// <summary>
        /// The single text box question page (user writes the value)
        /// </summary>
        QuestionSingleTextBox,

        #endregion
    }
}

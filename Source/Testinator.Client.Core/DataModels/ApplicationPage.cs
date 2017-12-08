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
        None = 0,

        /// <summary>
        /// The initial login page
        /// </summary>
        Login = 1,

        /// <summary>
        /// The page which waits for the test from server app
        /// </summary>
        WaitingForTest = 2,

        #region Question Pages

        /// <summary>
        /// The multiple choice question page (ABCD)
        /// </summary>
        QuestionMultipleChoice = 10,

        /// <summary>
        /// The multiple checkboxes question page (option checked or not)
        /// </summary>
        QuestionMultipleCheckboxes = 11,

        /// <summary>
        /// The single text box question page (user writes the value)
        /// </summary>
        QuestionSingleTextBox = 12,

        #endregion
    }
}

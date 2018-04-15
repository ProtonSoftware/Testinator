namespace Testinator.Core
{
    /// <summary>
    /// Question type as an enum
    /// </summary>
    public enum QuestionType
    {
        /// <summary>
        /// Empty or invalid question
        /// </summary>
        None,
        
        /// <summary>
        /// Multiple choice question, mostly A, B, C or D
        /// </summary>
        MultipleChoice,

        /// <summary>
        /// Select multiple answers from a list
        /// </summary>
        MultipleCheckboxes,

        /// <summary>
        /// The answer to the question is a string
        /// </summary>
        SingleTextBox,

    }
}

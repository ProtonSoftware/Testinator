namespace Testinator.Core
{
    /// <summary>
    /// Question type as an enum
    /// </summary>
    public enum QuestionType
    {
        /// <summary>
        /// Default no question type (used when we wanna hide something which base on question type)
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

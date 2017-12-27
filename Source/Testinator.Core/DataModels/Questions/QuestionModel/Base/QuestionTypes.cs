namespace Testinator.Core
{
    /// <summary>
    /// Question type as an enum
    /// </summary>
    public enum QuestionType
    {
        /// <summary>
        /// Default no question type 
        /// NOTE: Used when we wanna hide something which base on question type
        ///       Set at 5 because we have 3 question types and combobox base on integer number of type
        /// </summary>
        None = 5,

        /// <summary>
        /// Multiple choice question, mostly A, B, C or D
        /// </summary>
        MultipleChoice = 0,

        /// <summary>
        /// Select multiple answers from a list
        /// </summary>
        MultipleCheckboxes = 1,

        /// <summary>
        /// The answer to the question is a string
        /// </summary>
        SingleTextBox = 2,

    }
}

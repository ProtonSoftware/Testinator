namespace Testinator.Core
{ 
    /// <summary>
    /// The types of exceptions that may happen
    /// </summary>
    public enum QuestionExceptionTypes
    {
        /// <summary>
        /// Index of the correct answer is wrong
        /// </summary>
        WrongIndex,

        /// <summary>
        /// Point score for the question is less than 0
        /// </summary>
        PointScoreLessThanZero,

        /// <summary>
        /// String used for the task, or anything else is null or empty
        /// </summary>
        NullOrEmptyString,

        /// <summary>
        /// Not enough options for the question
        /// At least 2 for multiple choice question
        /// At least 1 for multiple chcekboxes question
        /// </summary>
        NotEnoughOptions,
    }
}

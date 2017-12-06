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
        PointScoreLessThan0,

        /// <summary>
        /// String used for the task, or anything else is null or empty
        /// </summary>
        NullOrEmptyString,

        /// <summary>
        /// For multiple choice there must be at leats 2 options
        /// </summary>
        NotEnoughOptions,
    }
}

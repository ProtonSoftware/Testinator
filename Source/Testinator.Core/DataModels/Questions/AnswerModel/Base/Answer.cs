namespace Testinator.Core
{
    /// <summary>
    /// The base class for every answer
    /// </summary>
    public abstract class Answer
    {
        /// <summary>
        /// The type of the question this answer is for
        /// </summary>
        public QuestionType Type { get; protected set; }
    }
}

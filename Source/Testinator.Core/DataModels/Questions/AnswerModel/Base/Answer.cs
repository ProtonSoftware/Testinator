namespace Testinator.Core
{
    /// <summary>
    /// The base model for all the answers for the questions
    /// </summary>
    public abstract class Answer
    {
        /// <summary>
        /// The type of the question this answer is for
        /// </summary>
        public QuestionTypes Type { get; protected set; }
    }
}

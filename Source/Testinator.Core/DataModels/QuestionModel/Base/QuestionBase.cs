namespace Testinator.Core
{
    /// <summary>
    /// A base model for all questions
    /// </summary>
    public abstract class Question
    {
        /// <summary>
        /// The type of this question
        /// </summary>
        public QuestionTypes Type { get; protected set; }
    }
}

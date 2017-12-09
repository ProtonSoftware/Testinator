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
        public QuestionTypes Type { get; protected set; }

        /// <summary>
        /// The ID to know which question is this answer attached to
        /// </summary>
        public int ID { get; set; }
    }
}

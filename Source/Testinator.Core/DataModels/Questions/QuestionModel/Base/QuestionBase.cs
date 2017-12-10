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
        public QuestionType Type { get; protected set; }

        /// <summary>
        /// Identifier of this question
        /// Usefull for matching answers to questions
        /// </summary>
        public int ID { get; set; }
    }
}

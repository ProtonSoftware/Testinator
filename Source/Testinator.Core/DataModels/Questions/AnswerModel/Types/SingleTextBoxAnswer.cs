namespace Testinator.Core
{
    /// <summary>
    /// The answer for multiple choice question
    /// </summary>
    public class SingleTextBoxAnswer : Answer
    {
        #region Public Properties

        /// <summary>
        /// The answer itself
        /// </summary>
        public string Answer { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default construcotr
        /// </summary>
        /// <param name="question">The question this answer is for</param>
        /// <param name="answer">The answer for the question given by the user</param>
        public SingleTextBoxAnswer(SingleTextBoxQuestion question, string answer)
        {
            Type = QuestionType.SingleTextBox;
            ID = question.ID;
            Answer = answer;
        }
        
        #endregion
    }
}

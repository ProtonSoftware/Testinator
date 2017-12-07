namespace Testinator.Core
{
    /// <summary>
    /// The answer for multiple choice question
    /// </summary>
    public class SingleTextBoxAnswer : Answer
    {
        #region Public Properties

        /// <summary>
        /// Keeps this answer connected with the question
        /// </summary>
        public int ID { get; set; }

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
        public SingleTextBoxAnswer(SingleTextBoxQuestion question)
        {
            Type = QuestionTypes.SingleTextBox;
            ID = question.ID;
        }
        
        #endregion
    }
}

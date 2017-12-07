namespace Testinator.Core
{
    /// <summary>
    /// The answer for multiple choice question
    /// </summary>
    public class MultipleChoiceAnswer : Answer
    {
        #region Public Properties

        /// <summary>
        /// Keeps this answer connected with the question
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Index of the selected answer
        /// </summary>
        public int SelectedAnswerIdx { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default construcotr
        /// </summary>
        /// <param name="question">The question this answer is for</param>
        public MultipleChoiceAnswer(MultipleChoiceQuestion question)
        {
            Type = QuestionTypes.MultipleChoice;
            ID = question.ID;
        }
        
        #endregion
    }
}

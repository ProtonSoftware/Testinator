namespace Testinator.Core
{
    /// <summary>
    /// The answer for multiple choice question
    /// </summary>
    public class MultipleChoiceAnswer : Answer
    {
        #region Public Properties

        /// <summary>
        /// Index of the selected answer
        /// </summary>
        public int SelectedAnswerIdx { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="question">The question this answer is for</param>
        /// <param name="AnswerIdx">The answer for this question</param>
        public MultipleChoiceAnswer(MultipleChoiceQuestion question, int AnswerIdx) 
        {
            Type = QuestionTypes.MultipleChoice;
            ID = question.ID;
            SelectedAnswerIdx = AnswerIdx;
        }
        
        #endregion
    }
}

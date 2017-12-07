using System.Collections.Generic;

namespace Testinator.Core
{
    /// <summary>
    /// The answer for multiple checkboxes question
    /// </summary>
    public class MultipleCheckboxesAnswer : Answer
    {
        #region Public Properties

        /// <summary>
        /// Keeps this answer connected with the question
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The answer itself
        /// </summary>
        public List<bool> Answers { get; set; } = new List<bool>();

        #endregion

        #region Constructor

        /// <summary>
        /// Default construcotr
        /// </summary>
        /// <param name="question">The question this answer is for</param>
        /// <param name="answer">The answer given by the user</param>
        public MultipleCheckboxesAnswer(MultipleCheckboxesQuestion question, List<bool> answer)
        {
            Type = QuestionTypes.MultipleCheckboxes;
            ID = question.ID;
            Answers = answer;
        }
        
        #endregion
    }
}

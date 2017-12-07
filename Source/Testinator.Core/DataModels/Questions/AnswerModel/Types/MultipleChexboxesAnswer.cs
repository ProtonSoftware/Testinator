using System.Collections.Generic;

namespace Testinator.Core
{
    /// <summary>
    /// The answer for multiple checkboxes question
    /// </summary>
    public class MultipleChexboxesAnswer : Answer
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
        public MultipleChexboxesAnswer(MultipleCheckboxesQuestion question)
        {
            Type = QuestionTypes.MultipleCheckboxes;
            ID = question.ID;

        }
        
        #endregion
    }
}

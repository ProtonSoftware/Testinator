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
        /// The answer itself
        /// </summary>
        public List<bool> Answers { get; set; } = new List<bool>();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="answer">The answer to the question</param>
        public MultipleCheckboxesAnswer(List<bool> answer)
        {
            Type = QuestionType.MultipleCheckboxes;
            Answers = answer;
        }
        
        #endregion
    }
}

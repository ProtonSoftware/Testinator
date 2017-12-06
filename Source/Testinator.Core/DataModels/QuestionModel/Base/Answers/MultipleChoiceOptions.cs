using System.Collections.Generic;

namespace Testinator.Core
{
    /// <summary>
    /// Contains options and correct answer for multiple choice question
    /// </summary>
    public class MultipleChoiceOptions : OptionsBase
    {
        /// <summary>
        /// Options for the question like, A, B, C etc.
        /// </summary>
        public List<string> Options { get; set; }

        /// <summary>
        /// Number of options available
        /// </summary>
        public int OptionsCount => Options.Count;

        /// <summary>
        /// The index in the table that contains the correct answer
        /// </summary>
        public int CorrectAnswerIndex { get; set; }
    }
}

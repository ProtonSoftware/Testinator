using System;

namespace Testinator.Core
{
    #region OLD

    /// <summary>
    /// The answer for multiple choice question
    /// </summary>
    [Serializable]
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
        /// <param name="AnswerIdx">The answer for this question</param>
        public MultipleChoiceAnswer(int AnswerIdx) 
        {
            Type = QuestionType.MultipleChoice;
            SelectedAnswerIdx = AnswerIdx;
        }
        
        #endregion
    }

    #endregion

    #region NEW

    /// <summary>
    /// The answer for <see cref="MultipleChoiceQuestionXXX"/>
    /// </summary>
    [Serializable]
    public class MultipleChoiceAnswerXXX : AnswerXXX
    {
        #region Private Members

        /// <summary>
        /// Indicates no answer given for this question
        /// </summary>
        private const int NoAnswerIndex = -1;

        #endregion

        #region Public Properties

        /// <summary>
        /// Index of the answer selected by the user
        /// </summary>
        public int SelectedAnswerIndex { get; set; } = NoAnswerIndex;

        /// <summary>
        /// The type of question this answer is for
        /// </summary>
        public override QuestionType Type => QuestionType.MultipleChoice;

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks if this answer is empty, meaning that the user did not manage to answer this question
        /// </summary>
        /// <returns>True if this answer is empty, otherwise false</returns>
        public override bool IsEmpty() => SelectedAnswerIndex == NoAnswerIndex;
        
        #endregion
    }

    #endregion
}

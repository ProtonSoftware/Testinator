using System;

namespace Testinator.Core
{
    /// <summary>
    /// Is thrown when trying to set correct answer for the question 
    /// </summary>
    public class QuestionException : Exception
    {
        #region Public Properties

        public QuestionExceptionTypes Type { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Throws exception with the given type
        /// </summary>
        /// <param name="type">Type of this exception</param>
        public QuestionException(QuestionExceptionTypes type)
        {
            Type = type;
        }
        

        #endregion
    }
}

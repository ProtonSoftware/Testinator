using System;

namespace Testinator.Core
{
    /// <summary>
    /// The question base question interface 
    /// </summary>
    public interface IQuestion
    {
        /// <summary>
        /// Gets the task of this question as html code
        /// </summary>
        /// <returns>The html representation of this question's task</returns>
        //HtmlCode GetTask();

        /// <summary>
        /// Gets the number of points for a good answer specific to this question
        /// NOTE: As there are many types of question this can be highly customized
        /// </summary>
        /// <returns>Gets points score in a string format so that it can be easily displayed</returns>
        string GetDisplayPointScore();

        /// <summary>
        /// Checks if the given answer is correct so the points can be given for correct one
        /// </summary>
        /// <param name="Answer">The answer to be checked</param>
        /// <returns>True if the answer is correct, otherwise false</returns>
        bool CheckAnswer(IAnswer Answer);
    }
}

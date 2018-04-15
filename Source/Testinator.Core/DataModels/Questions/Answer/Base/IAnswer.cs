namespace Testinator.Core
{
    /// <summary>
    /// Foundation of any Answer class, which is used to store the answer
    /// given by the user while taking a test
    /// </summary> 
    public interface IAnswer
    {
        /// <summary>
        /// Checks if the answer is empty, meaning the user did not answer this question
        /// </summary>
        /// <returns>True if empty; otherwise, false</returns>
        bool IsEmpty();
    }
}

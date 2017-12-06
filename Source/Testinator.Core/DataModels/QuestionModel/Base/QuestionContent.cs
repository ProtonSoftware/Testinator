namespace Testinator.Core
{
    /// <summary>
    /// The content of the question
    /// </summary>
    public class QuestionContent
    {
        /// <summary>
        /// The task of the question
        /// </summary>
        public string Task { get; set; }

        /// <summary>
        /// Options for multiple choice answers or any other
        /// Contains also the correct answer to the question
        /// </summary>
        public OptionsBase Options { get; set; }

    }
}

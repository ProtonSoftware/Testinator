namespace Testinator.Core
{ 
    /// <summary>
    /// The model for text entry question
    /// </summary>
    public class TextEntryQuestion : OptionsBase
    {
        /// <summary>
        /// The correct answer for the question
        /// </summary>
        public string CorrectAnswer { get; set; }
    }
}

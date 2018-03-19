namespace Testinator.Core
{
    /// <summary>
    /// Details for a decision box dialog
    /// User has two options: Accept or Cancel
    /// </summary>
    public class DecisionDialogViewModel : BaseDialogViewModel
    {
        /// <summary>
        /// The message to display
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The text to use for the accept button
        /// </summary>
        public string AcceptText { get; set; }

        /// <summary>
        /// The text to use for the cancel button
        /// </summary>
        public string CancelText { get; set; }

        /// <summary>
        /// Used after showing a message
        /// Keeps user's answer, whether he agreed with the statement or not
        /// True means that he accept the message
        /// False means that he declined
        /// </summary>
        public bool UserResponse { get; set; }
    }
}

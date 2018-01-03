namespace Testinator.Server.Core
{
    /// <summary>
    /// Details for a result box dialog
    /// where user decide if he agrees with the message or not
    /// </summary>
    public class ResultBoxDialogViewModel : BaseDialogViewModel
    {
        /// <summary>
        /// The message to display
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The text to use for the agree button
        /// </summary>
        public string AgreeText { get; set; } = "Tak";

        /// <summary>
        /// The text to use for the cancel button
        /// </summary>
        public string CancelText { get; set; } = "Nie";

        /// <summary>
        /// Used after showing a message
        /// Keeps user's answer, whether he agreed with the statement or not
        /// </summary>
        public bool HasUserAgreed { get; set; } = false;
    }
}

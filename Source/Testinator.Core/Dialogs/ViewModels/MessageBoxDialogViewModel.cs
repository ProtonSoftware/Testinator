﻿namespace Testinator.Core
{
    /// <summary>
    /// Details for a message box dialog
    /// </summary>
    public class MessageBoxDialogViewModel : BaseDialogViewModel
    {
        /// <summary>
        /// The message to display
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The text to use for the OK button
        /// </summary>
        public string OkText { get; set; } = "OK";

        /// <summary>
        /// The type (style) of this message box
        /// </summary>
        public MessageTypes Type { get; set; } = MessageTypes.Information;
    }
}

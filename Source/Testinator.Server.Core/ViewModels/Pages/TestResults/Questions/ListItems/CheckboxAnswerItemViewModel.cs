using System;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The view model for checkbox answer item
    /// </summary>
    public class CheckboxAnswerItemViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Text of an answer to display
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// A flag indicating if the checkbox is checked
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// Indicates if the users answer is correct
        /// </summary>
        public bool IsCorrect { get; set; }

        #endregion
    }
}

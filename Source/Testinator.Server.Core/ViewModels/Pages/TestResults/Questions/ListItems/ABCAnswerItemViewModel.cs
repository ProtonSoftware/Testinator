using System;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The view model for multiple choice (ABCD) question item
    /// </summary>
    public class ABCAnswerItemViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Text of an answer to display
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Ordered letter for the answer (A, B, C, D or E)
        /// </summary>
        public string Letter { get; set; }

        /// <summary>
        /// A flag indicating if this item is selected
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// The index of this option, starts at 1
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Indicates if the answer given by the user is forrect
        /// Makes sense only if the read only mode is enabled
        /// </summary>
        public bool IsAnswerCorrect { get; set; }

        /// <summary>
        /// Indicates if this answer was given by the user
        /// Makes sense only if the read only mode is enabled
        /// </summary>
        public bool IsAnswerGivenByTheUser { get; set; }

        /// <summary>
        /// Indicates if this item's background should be red
        /// When the answer is given by the user and is not correct
        /// </summary>
        public bool RedBackground => IsAnswerGivenByTheUser && !IsAnswerCorrect;

        #endregion
    }
}

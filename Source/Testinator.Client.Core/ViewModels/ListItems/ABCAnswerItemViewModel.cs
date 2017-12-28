using System;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Client.Core
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

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ABCAnswerItemViewModel()
        {
        }

        #endregion

    }
}

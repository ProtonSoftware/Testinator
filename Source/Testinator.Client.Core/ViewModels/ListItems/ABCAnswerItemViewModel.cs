using System;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Client.Core
{
    /// <summary>
    /// The view model for multiple choice (ABCD) question item
    /// </summary>
    public class ABCAnswerItemViewModel
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

        #endregion

        #region Commands

        /// <summary>
        /// The command to mark/unmark this item as selected
        /// </summary>
        public ICommand SelectItemCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ABCAnswerItemViewModel()
        {
            // Create commands
            SelectItemCommand = new RelayCommand(SelectItem);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Marks/unmarks this item as selected
        /// </summary>
        private void SelectItem()
        {
            // Simple toggle item selected flag 
            IsSelected ^= true;
        }

        #endregion
    }
}

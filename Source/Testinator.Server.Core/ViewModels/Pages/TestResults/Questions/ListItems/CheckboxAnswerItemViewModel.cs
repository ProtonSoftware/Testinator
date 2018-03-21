using System;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The view model for checkbox answer item
    /// </summary>
    public class CheckboxAnswerItemViewModel
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
        /// Indicates whether the view should be enabled for changes
        /// ReadOnly mode is used while presenting the result to the user
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Indicates if the user's answer is correct
        /// Makes sense only if the readonly mode is enabled
        /// Used to make the background red or green
        /// </summary>
        public bool IsCorrect { get; set; }

        public bool IsGreen => IsReadOnly && IsCorrect;
        public bool IsRed => IsReadOnly && !IsCorrect;

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
        public CheckboxAnswerItemViewModel()
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
            // Simple toggle item checked flag 
            IsChecked ^= true;
        }

        #endregion
    }
}

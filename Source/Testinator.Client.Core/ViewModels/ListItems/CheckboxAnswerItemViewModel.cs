using System;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Client.Core
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
        /// Indicates if the checkbox should be checked to earn points
        /// </summary>
        public bool ShouldBeChecked { get; set; }

        /// <summary>
        /// A flag indicating if the checkbox is checked
        /// </summary>
        public bool IsChecked { get; set; }

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
            // Simple toogle item checked flag 
            IsChecked ^= true;
        }

        #endregion
    }
}

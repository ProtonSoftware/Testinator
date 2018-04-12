using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The view model for any <see cref="Question"/> to show it in the list control
    /// </summary>
    public class QuestionListItemViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The id of this item
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Display number for this item
        /// </summary>
        public string DisplayNumber => (ID + 1).ToString();
        
        /// <summary>
        /// The type of this question item so show the icon correctly
        /// </summary>
        public QuestionType Type { get; set; }

        /// <summary>
        /// Indicates if this item is currently selected
        /// </summary>
        public bool IsSelected { get; set; } = false;

        /// <summary>
        /// The task of this question to display
        /// </summary>
        public string Task { get; set; }

        #endregion
    }
}

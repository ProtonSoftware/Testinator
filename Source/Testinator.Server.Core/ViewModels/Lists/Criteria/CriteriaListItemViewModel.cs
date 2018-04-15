using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// A view model desribing <see cref="GradingPercentage"/> object
    /// </summary>
    public class CriteriaListItemViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The if of this item in the list
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The grading this model is based on
        /// </summary>
        public GradingPercentage Grading { get; set; }

        /// <summary>
        /// Indicates if this item is currently selected
        /// </summary>
        public bool IsSelected { get; set; } = false;

        #endregion
    }
}

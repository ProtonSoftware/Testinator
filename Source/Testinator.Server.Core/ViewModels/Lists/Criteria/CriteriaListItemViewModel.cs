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
        /// The grading this model is based on
        /// </summary>
        public GradingPercentage Grading { get; set; }

        /// <summary>
        /// Indicates if this item is currently selected
        /// </summary>
        public bool IsSelected { get; set; } = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Default construcotr
        /// </summary>
        /// <param name="grading">The object this view model is based on</param>
        public CriteriaListItemViewModel(GradingPercentage grading)
        {
            Grading = grading;
        }

        #endregion
    }
}

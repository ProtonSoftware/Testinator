using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// View model for any <see cref="Test"/> to show it in list control
    /// </summary>
    public class TestListItemViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The test itself
        /// </summary>
        public Test Test { get; set; }

        /// <summary>
        /// The id of this test
        /// NOTE: starts from 1, not 0!
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Indicates if this test is currently selected
        /// </summary>
        public bool IsSelected { get; set; } = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestListItemViewModel()
        {

        }

        #endregion
    }
}

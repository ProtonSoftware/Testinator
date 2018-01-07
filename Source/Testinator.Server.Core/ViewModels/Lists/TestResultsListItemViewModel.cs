using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// A view model for any itme in test results list control
    /// </summary>
    public class TestResultsListItemViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The name of the test corresponding to this result
        /// </summary>
        public string TestName { get; set; }

        /// <summary>
        /// The test this results was saved
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Indicates if this item is currently selected
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// The index of this item in the list
        /// </summary>
        public int Index { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="results">The results this viewmodel will be based on</param>
        /// <param name="index">THe index of this item in the list</param>
        public TestResultsListItemViewModel(TestResults results, int index)
        {
            TestName = results.Test.Name;
            Date = results.Date.ToLongDateString();
            Index = index;
        }

        #endregion
    }
}

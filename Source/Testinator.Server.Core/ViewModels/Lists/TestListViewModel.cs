using System.Collections.ObjectModel;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The view model for the test list
    /// </summary>
    public class TestListViewModel : BaseViewModel
    {
        #region Singleton

        /// <summary>
        /// Single instance of this view model
        /// </summary>
        public static TestListViewModel Instance { get; private set; } = new TestListViewModel();

        #endregion

        #region Public Properties

        /// <summary>
        /// List of items (tests) in this test list
        /// </summary>
        public ObservableCollection<TestListItemViewModel> Items { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestListViewModel()
        {
            // Load every criteria at the start
            LoadItems();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads the criteria to the list
        /// </summary>
        public void LoadItems()
        {
            // Load the list every test from bin files
            var list = FileReaders.BinReader.ReadAllTests();

            // Rewrite list to the collection
            Items = new ObservableCollection<TestListItemViewModel>();
            int i = 1;
            list.ForEach( (x) =>
            {
                Items.Add(new TestListItemViewModel()
                {
                    ID = i,
                    Test = x,
                });
                i++;
            });
        }

        #endregion
    }
}

using System.Collections.ObjectModel;
using System.Windows.Input;
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

        #region Commands

        /// <summary>
        /// The command to choose a test from the list
        /// </summary>
        public ICommand ChooseTestCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestListViewModel()
        {
            // Create commands
            ChooseTestCommand = new RelayParameterizedCommand((param) => ChooseTest(param));

            // Load every criteria at the start
            LoadItems();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Chooses a test from the list
        /// </summary>
        private void ChooseTest(object param)
        {
            // Cast the parameter
            var testID = int.Parse(param.ToString());

            // Load test based on that
            IoCServer.TestHost.BindTest(Items[testID - 1].Test);

            // Mark all items not selected
            foreach (var item in Items)
            {
                item.IsSelected = false;
            }

            // Select the one that has been clicked
            Items[testID - 1].IsSelected = true;
        }

        #endregion

        #region Public Helpers

        /// <summary>
        /// Checks if there is any test selected
        /// </summary>
        /// <returns>True if there is a test selected,
        /// False if not</returns>
        public bool IsAnyTestSelected()
        {
            foreach (var item in Items)
            {
                if (item.IsSelected)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Loads tests to the list
        /// </summary>
        public void LoadItems()
        {
            // Load the list every test from bin files
            var list = FileReaders.BinReader.ReadAllTests();

            // Rewrite list to the collection
            Items = new ObservableCollection<TestListItemViewModel>();
            var i = 1;
            list.ForEach((x) =>
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

using System;
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

        /// <summary>
        /// The test binary file reader which loads tests from local folder
        /// </summary>
        public BinaryReader TestFileReader { get; private set; } = new BinaryReader(SaveableObjects.Test);

        /// <summary>
        /// Indicates if there is any test selected
        /// </summary>
        public bool IsAnySelected { get; private set; }

        #endregion

        /// <summary>
        /// Fired when an item from the list gets selected
        /// </summary>
        public event Action ItemSelected = () => { };

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

            // Load every test at the start
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

            // Indicate that a test is now selected
            IsAnySelected = true;

            // Fire an event
            ItemSelected.Invoke();
        }

        #endregion

        #region Public Helpers

        /// <summary>
        /// Loads tests to the list
        /// </summary>
        public void LoadItems()
        {
            // Load the list every test from bin files
            var list = TestFileReader.ReadAllTests();

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

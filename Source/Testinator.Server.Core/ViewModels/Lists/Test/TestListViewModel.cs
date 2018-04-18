using System;
using System.Collections.Generic;
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

        #region Public Constants 
        
        /// <summary>
        /// Indicates no selection
        /// </summary>
        public const int NothingSelected = -1;

        #endregion

        #region Privates Members

        /// <summary>
        /// Indicates currently selected item's index
        /// </summary>
        private int mCurrentlySelectedItemIndex = NothingSelected;

        #endregion

        #region Public Properties

        /// <summary>
        /// List of items (tests) in this test list
        /// </summary>
        public ObservableCollection<TestListItemViewModel> Items { get; private set; }

        /// <summary>
        /// The test binary file reader which loads tests from local folder
        /// </summary>
        public BinaryReader TestFileReader { get; private set; } = new BinaryReader(SaveableObjects.Test);

        /// <summary>
        /// Indicates if there is any test selected
        /// </summary>
        public bool IsAnySelected { get; private set; }

        /// <summary>
        /// Indicates currently selected item
        /// NOTE: null if nothing is selected
        /// </summary>
        public Test SelectedItem { get; private set; }

        #endregion

        /// <summary>
        /// Fired when selection in this list chages
        /// NOTE: not invoked if this same item is selected multiple times
        /// </summary>
        public event Action SelectionChanged = () => { };

        /// <summary>
        /// Fired when item gets selected 
        /// </summary>
        public event Action<Test> ItemSelected = (o) => { };

        #region Commands

        /// <summary>
        /// The command to select an item
        /// </summary>
        public ICommand SelectItemCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestListViewModel()
        {
            // Create commands
            SelectItemCommand = new RelayParameterizedCommand((param) => SelectItem(param));
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Select an item from the list
        /// </summary>
        private void SelectItem(object param)
        {
            // Cast the parameter
            var newSelectedItemIndex = int.Parse(param.ToString());

            // If the same item got selected there is no more to do
            if (mCurrentlySelectedItemIndex == newSelectedItemIndex)
                return;

            // Unselect last item
            if (mCurrentlySelectedItemIndex != NothingSelected)
                Items[mCurrentlySelectedItemIndex].IsSelected = false;

            // Select the one that has been clicked
            Items[newSelectedItemIndex].IsSelected = true;

            // Save new selected item index
            mCurrentlySelectedItemIndex = newSelectedItemIndex;

            // Indicate that there is an item selected
            IsAnySelected = true;

            // Set selected item
            SelectedItem = Items[newSelectedItemIndex].Test;

            // Fire the events
            SelectionChanged.Invoke();
            ItemSelected.Invoke(SelectedItem);
        }

        #endregion

        #region Public Helpers

        /// <summary>
        /// Loads tests to the list
        /// </summary>
        public void LoadItems()
        {
            var ItemsOnHardDrive = new List<Test>();

            try
            {
                // Try to load the list of every test from bin files
                ItemsOnHardDrive = TestFileReader.ReadFile<Test>();
            }
            catch (Exception ex)
            {
                // If an error occured, show info to the user
                IoCServer.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Błąd wczytywania",
                    Message = "Nie udało się wczytać dostępnych testów." +
                              "\nTreść błędu: " + ex.Message,
                    OkText = "Ok"
                });

                IoCServer.Logger.Log("Unable to read tests from local folder, error message: " + ex.Message);
            }

            // Rewrite list to the collection
            Items = new ObservableCollection<TestListItemViewModel>();
            for (var i = 0; i < ItemsOnHardDrive.Count; i++)
            {
                Items.Add(new TestListItemViewModel()
                {
                    ID = i,
                    Test = ItemsOnHardDrive[i],
                });
            }

            mCurrentlySelectedItemIndex = NothingSelected;
            SelectedItem = null;
            IsAnySelected = false;

            SelectionChanged.Invoke();
        }

        #endregion
    }
}

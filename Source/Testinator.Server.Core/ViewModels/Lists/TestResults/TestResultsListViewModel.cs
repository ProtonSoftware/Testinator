using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// View model for a test results list
    /// </summary>
    public class TestResultsListViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// All test results found on the machine
        /// </summary>
        public ObservableCollection<TestResultsListItemViewModel> Items { get; private set; } = new ObservableCollection<TestResultsListItemViewModel>();

        /// <summary>
        /// The results binary file reaer which reads results from local folder
        /// </summary>
        public BinaryReader ResultsFileReader { get; private set; } = new BinaryReader(SaveableObjects.Results);

        /// <summary>
        /// The list of test results
        /// </summary>
        public List<ServerTestResults> ResultsList { get; private set; } = new List<ServerTestResults>();

        #endregion

        #region Public Events

        /// <summary>
        /// Fired when an item is selected
        /// </summary>
        public event Action<ServerTestResults> ItemSelected = (x) => { };

        #endregion

        #region Commands

        /// <summary>
        /// The command to select item from the list
        /// </summary>
        public ICommand SelectCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestResultsListViewModel()
        {
            // Create commands
            SelectCommand = new RelayParameterizedCommand(Select);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Selects an item from the list
        /// </summary>
        /// <param name="param">The index of the item</param>
        private void Select(object param)
        {
            // Cast the parameter
            var index = int.Parse(param.ToString());

            // Select the item with that index
            SelectItem(index);
            ItemSelected.Invoke(ResultsList[index]);
        }

        #endregion

        #region Public Helpers

        /// <summary>
        /// Load the control with item read from the disk
        /// </summary>
        public void LoadItems()
        {
            try
            {
                // Try to load the list of every result from bin files
                ResultsList = ResultsFileReader.ReadFile<ServerTestResults>();
            }
            catch (Exception ex)
            {
                /*
                // If an error occured, show info to the user
                IoCServer.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Błąd wczytywania",
                    Message = "Nie udało się wczytać dostępnych rezultatów." +
                              "\nTreść błędu: " + ex.Message,
                    OkText = "Ok"
                });*/

                IoCServer.Logger.Log("Unable to read results from local folder, error message: " + ex.Message);
            }

            // Rewrite list to the collection
            Items = new ObservableCollection<TestResultsListItemViewModel>();
            var indexer = 0;
            foreach (var result in ResultsList)
            {
                Items.Add(new TestResultsListItemViewModel(result, indexer));
                indexer++;
            }
        }

        /// <summary>
        /// Checks if there is any item selected in the list
        /// </summary>
        /// <returns></returns>
        public bool IsAnySelected()
        {
            foreach (var item in Items)
            {
                if (item.IsSelected)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks which item is currently selected
        /// </summary>
        /// <returns>Null if there isn't any item selected,
        /// otherwise return the currently selected item</returns>
        public ServerTestResults SelectedItem()
        {
            foreach (var item in Items)
            {
                if (item.IsSelected)
                {
                    var idx = item.Index;
                    return ResultsList[idx];
                }
            }

            return null;
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Marks all the items not selected
        /// </summary>
        private void UncheckAll()
        {
            foreach (var item in Items)
                item.IsSelected = false;
        }            

        /// <summary>
        /// Selects an item
        /// </summary>
        /// <param name="index">The index of the item</param>
        private void SelectItem(int index)
        {
            if (index >= Items.Count)
                return;

            UncheckAll();

            Items[index].IsSelected = true;
        }

        #endregion
    }
}

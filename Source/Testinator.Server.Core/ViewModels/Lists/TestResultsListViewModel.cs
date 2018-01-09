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
        public ObservableCollection<TestResultsListItemViewModel> Items { get; set; } = new ObservableCollection<TestResultsListItemViewModel>();

        #endregion

        #region Public Methods

        /// <summary>
        /// Load the control with item read from the disk
        /// </summary>
        public void LoadItems(List<TestResults> results)
        {
            Items = new ObservableCollection<TestResultsListItemViewModel>();

            var indexer = 0;
            foreach (var result in results)
            {
                Items.Add(new TestResultsListItemViewModel(result, indexer));
                indexer++;
            }
        }

        #endregion

        #region Public Commands

        /// <summary>
        /// The command to select item from the list
        /// </summary>
        public ICommand SelectCommand { get; private set; }

        #endregion

        #region Command Methods

        /// <summary>
        /// Selects and item from the list
        /// </summary>
        /// <param name="index">The index of the item (intiger)</param>
        private void Select(object index)
        {
            int indexInt;

            try
            {
                indexInt = (int)index;
            }
            catch
            {
                return;
            }

            SelectItem(indexInt);      
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestResultsListViewModel()
        {
            SelectCommand = new RelayParameterizedCommand(Select);
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

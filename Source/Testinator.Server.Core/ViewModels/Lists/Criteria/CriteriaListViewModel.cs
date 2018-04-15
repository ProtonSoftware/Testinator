using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The view model for the criteria list
    /// </summary>
    public class CriteriaListViewModel : BaseViewModel
    {
        #region Public Constats

        /// <summary>
        /// Indicates that nothing is selected
        /// </summary>
        private const int NothingSelected = -1;

        #endregion

        #region Singleton

        /// <summary>
        /// Single instance of this view model
        /// </summary>
        public static CriteriaListViewModel Instance { get; private set; } = new CriteriaListViewModel();

        #endregion

        #region Private Members

        /// <summary>
        /// Currently selected item index
        /// </summary>
        private int mCurrentlySelectedIndex;

        #endregion

        #region Public Properties

        /// <summary>
        /// List of items (criterias) in this criteria list
        /// </summary>
        public ObservableCollection<CriteriaListItemViewModel> Items { get; private set; } = new ObservableCollection<CriteriaListItemViewModel>();

        /// <summary>
        /// The criteria xml file reader which loads criteria from local folder
        /// </summary>
        public XmlReader CriteriaFileReader { get; private set; } = new XmlReader(SaveableObjects.Grading);

        /// <summary>
        /// Indicates if the indicator should be visible after clicking an item
        /// </summary>
        public bool ShouldSelectIndicatorBeVisible { get; set; }

        #endregion

        #region Public Events

        /// <summary>
        /// Fired when an item is selected from the list
        /// </summary>
        public event Action<GradingPercentage> ItemSelected = (i) => { };

        #endregion

        #region Public Commands

        /// <summary>
        /// The command to select an item from the list
        /// </summary>
        public ICommand SelectItemCommand { get; private set; }

        #endregion

        #region Command Methods
        
        /// <summary>
        /// Selects an item from the list
        /// </summary>
        /// <param name="obj"></param>
        private void SelectItem(object obj)
        {
            var newSelectedIndex = (int)obj;

            if(ShouldSelectIndicatorBeVisible)
            {
                Items[mCurrentlySelectedIndex].IsSelected = false;
                Items[newSelectedIndex].IsSelected = true;
            }

            mCurrentlySelectedIndex = newSelectedIndex;

            ItemSelected.Invoke(Items[newSelectedIndex].Grading);
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public CriteriaListViewModel()
        {
            // Load every criteria at the start
            LoadItems();

            SelectItemCommand = new RelayParameterizedCommand(SelectItem);
        }

        #endregion

        #region Public Helpers

        /// <summary>
        /// Marks all the items unchecked
        /// </summary>
        public void UncheckAll()
        {
            // Simply mark every item as unselected
            foreach (var item in Items)
                item.IsSelected = false;
        }

        /// <summary>
        /// Loads the criteria to the list
        /// </summary>
        public void LoadItems()
        {
            var list = new List<GradingPercentage>();
            try
            {
                // Try to load the list of every criteria from xml files 
                list = CriteriaFileReader.ReadFile<GradingPercentage>();
            }
            catch (Exception ex)
            {
                // If an error occured, show info to the user
                IoCServer.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Błąd wczytywania",
                    Message = "Nie udało się wczytać dostępnych kryteriów." +
                              "\nTreść błędu: " + ex.Message,
                    OkText = "Ok"
                });

                IoCServer.Logger.Log("Unable to read criteria from local folder, error message: " + ex.Message);
            }

            // Rewrite list to the observable collection
            Items.Clear();


            var indexer = 0;
            foreach (var item in list)
            {
                Items.Add(new CriteriaListItemViewModel()
                {
                    Grading = item,
                    ID = indexer,
                });
                indexer++;
            }
        }

        #endregion
    }
}

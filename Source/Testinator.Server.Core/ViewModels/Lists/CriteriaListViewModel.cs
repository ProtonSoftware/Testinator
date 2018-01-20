using System.Collections.ObjectModel;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The view model for the criteria list
    /// </summary>
    public class CriteriaListViewModel : BaseViewModel
    {
        #region Singleton

        /// <summary>
        /// Single instance of this view model
        /// </summary>
        public static CriteriaListViewModel Instance { get; private set; } = new CriteriaListViewModel();

        #endregion

        #region Public Properties

        /// <summary>
        /// List of items (criterias) in this criteria list
        /// </summary>
        public ObservableCollection<CriteriaListItemViewModel> Items { get; set; }

        /// <summary>
        /// The criteria xml file reader which loads criteria from local folder
        /// </summary>
        public XmlReader CriteriaFileReader { get; private set; } = new XmlReader(SaveableObjects.Grading);

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public CriteriaListViewModel()
        {
            // Load every criteria at the start
            LoadItems();
        }

        #endregion

        #region Public Helpers

        /// <summary>
        /// Marks all the items unchecked
        /// </summary>
        public void UncheckAll()
        {
            // Simply mark every item as unselected
            foreach (var item in Items) item.IsSelected = false;
        }

        /// <summary>
        /// Loads the criteria to the list
        /// </summary>
        public void LoadItems()
        {
            // Load the list every criteria from xml files
            var list = CriteriaFileReader.ReadXmlGrading();

            // Rewrite list to the observable collection
            Items = new ObservableCollection<CriteriaListItemViewModel>();
            foreach (var item in list) Items.Add(new CriteriaListItemViewModel(item));
        }

        #endregion
    }
}

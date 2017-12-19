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
        public ObservableCollection<GradingExtended> Items { get; set; }

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

        #region Public Methods

        /// <summary>
        /// Loads the criteria to the list
        /// </summary>
        public void LoadItems()
        {
            // Load the list every criteria from xml files
            var list = FileReaders.XmlReader.ReadXmlGrading();

            // Rewrite list to the collection
            Items = new ObservableCollection<GradingExtended>();
            foreach (var item in list) Items.Add(item);
        }

        #endregion
    }
}

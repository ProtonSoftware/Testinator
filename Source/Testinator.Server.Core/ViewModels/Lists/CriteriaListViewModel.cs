using System.Collections.Generic;
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
        public static CriteriaListViewModel Instance => new CriteriaListViewModel();

        #endregion

        #region Public Properties

        /// <summary>
        /// List of items (criterias) in this criteria list
        /// </summary>
        public List<GradingExtended> Items { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public CriteriaListViewModel()
        {
            // Load every criteria at the start
            Items = FileReaders.XmlReader.ReadXmlGrading();
        }

        #endregion
    }
}

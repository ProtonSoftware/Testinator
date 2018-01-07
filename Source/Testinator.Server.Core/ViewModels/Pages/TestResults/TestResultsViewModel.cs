using System;
using System.Collections.Generic;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// A viewmodel for the results page
    /// </summary>
    public class TestResultsViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// View model for the list control
        /// </summary>
        public TestResultsListViewModel ListViewModel => new TestResultsListViewModel();

        /// <summary>
        /// The list of all results found on the machine
        /// </summary>
        public List<TestResults> Results { get; set; } = new List<TestResults>();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestResultsViewModel()
        {
            Results = FileReaders.BinReader.ReadAllResults();
            ListViewModel.LoadItems(Results);
            OnPropertyChanged(nameof(ListViewModel));
        }

        #endregion

        #region Private Helpers


        #endregion
    }
}

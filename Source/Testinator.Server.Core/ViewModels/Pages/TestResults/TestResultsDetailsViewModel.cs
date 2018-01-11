using System;
using System.Collections.Generic;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The view model of the resilts details page
    /// </summary>
    public class TestResultsDetailsViewModel : BaseViewModel
    {
        #region Public Proprties

        public List<ClientModelSerializable> Clients { get; private set; } = new List<ClientModelSerializable>();

        #endregion

        #region Public Commands

        /// <summary>
        /// The command to go back to the previous page
        /// </summary>
        public ICommand ReturnCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestResultsDetailsViewModel()
        {
            ReturnCommand = new RelayCommand(ReturnPreviousPage);
        }

        /// <summary>
        /// Constructs the viewmodel and loads it's properties with the given data
        /// </summary>
        /// <param name="results">Data to be loaded</param>
        public TestResultsDetailsViewModel(TestResults results)
        {
            LoadData(results);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Load this viewmodel with teh given data
        /// </summary>
        /// <param name="value"></param>
        public void LoadData(TestResults value)
        {
            if (value == null)
                return;

            Clients = value.Clients;
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Goes back to the previous page
        /// </summary>
        private void ReturnPreviousPage()
        {
            IoCServer.Application.GoToPage(ApplicationPage.TestResultsInitial);
        }

        #endregion
    }
}

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
        #region Private Members

        /// <summary>
        /// The results this viewmodel is based on
        /// </summary>
        private TestResults mTestResults = new TestResults();

        #endregion

        #region Public Properties

        /// <summary>
        /// Comment here..................................................................
        /// </summary>
        public List<ClientModelSerializable> Clients { get; private set; } = new List<ClientModelSerializable>();

        /// <summary>
        /// The name of the test the users took
        /// </summary>
        public string TestName => mTestResults.Test.Name;

        /// <summary>
        /// Status of the menu as a boolean
        /// </summary>
        public bool IsMenuExpanded { get; private set; } = false;

        #endregion

        #region Commands

        /// <summary>
        /// The command to go back to the previous page
        /// </summary>
        public ICommand ReturnCommand { get; private set; }

        /// <summary>
        /// The command to expand or hide menu
        /// </summary>
        public ICommand MenuToggleCommand { get; private set; }

        /// <summary>
        /// Changes the view to the students view
        /// </summary>
        public ICommand ChangeViewStudentsCommand { get; private set; }

        /// <summary>
        /// Changes the view to the questions view
        /// </summary>
        public ICommand ChangeViewQuestionsCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestResultsDetailsViewModel()
        {
            // Create commands
            CreateCommands();
        }

        /// <summary>
        /// Constructs the viewmodel and loads it's properties with the given data
        /// </summary>
        /// <param name="results">Data to be loaded</param>
        public TestResultsDetailsViewModel(TestResults results)
        {
            // Create commands
            CreateCommands();

            LoadData(results);
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

        /// <summary>
        /// Expands or hides the menu
        /// </summary>
        private void MenuToggle()
        {
            IsMenuExpanded ^= true;
        }

        /// <summary>
        /// Changes the view to the students view
        /// </summary>
        private void ChangeViewStudents()
        {
        }

        /// <summary>
        /// Changes the view to the questions view
        /// </summary>
        private void ChangeViewQuestions()
        {
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Load this viewmodel with given data
        /// </summary>
        /// <param name="value"></param>
        private void LoadData(TestResults value)
        {
            if (value == null)
                return;

            Clients = value.Clients;
            mTestResults = value;
        }

        /// <summary>
        /// Creates commands in this view model
        /// </summary>
        private void CreateCommands()
        {
            ReturnCommand = new RelayCommand(ReturnPreviousPage);
            MenuToggleCommand = new RelayCommand(MenuToggle);
            ChangeViewStudentsCommand = new RelayCommand(ChangeViewStudents);
            ChangeViewQuestionsCommand = new RelayCommand(ChangeViewQuestions);
        }

        #endregion
    }
}

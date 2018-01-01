using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The view model for edit existing test page
    /// </summary>
    public class TestEditorEditTestViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The test which is choosen by user on the list
        /// </summary>
        public Test CurrentTest => IoCServer.TestHost.Test;

        #endregion

        #region Commands

        /// <summary>
        /// The command to change page to adding test to which we provide data from existing test
        /// </summary>
        public ICommand ChangePageCommand { get; private set; }

        /// <summary>
        /// The command to delete test which is choosen in the list
        /// </summary>
        public ICommand DeleteTestCommand { get; private set; }

        #endregion

        #region Constructor

        public TestEditorEditTestViewModel()
        {
            // Create commands
            ChangePageCommand = new RelayCommand(ChangePage);
            DeleteTestCommand = new RelayCommand(DeleteTest);

            // Make sure that test list is up to date
            TestListViewModel.Instance.LoadItems();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Changes page to add new test page with data from selected test in this page
        /// </summary>
        private void ChangePage()
        {
            // Check if user has selected any test
            if (!TestListViewModel.Instance.IsAnyTestSelected())
                return;

            // Update the current test property to make sure its indicating the right test
            OnPropertyChanged(nameof(CurrentTest));

            // Create view model which contains selected test data
            var viewModel = new TestEditorAddNewTestViewModel(CurrentTest)
            {
                Name = CurrentTest.Name,
                Duration = CurrentTest.Duration.Minutes.ToString()
            };

            // Go to test editor add new test page, where user can edit this test
            IoCServer.Application.GoToPage(ApplicationPage.TestEditorAddTest, viewModel);
        }

        /// <summary>
        /// Deletes test from the list
        /// </summary>
        private void DeleteTest()
        {
            // Check if user has selected any test
            if (!TestListViewModel.Instance.IsAnyTestSelected())
                return;

            // Update the current test property to make sure its indicating the right test
            OnPropertyChanged(nameof(CurrentTest));

            // TODO: Show message box to user if he is sure he wants to delete the test

            // Finally delete selected test
            FileWriters.BinWriter.DeleteFile(CurrentTest);

            // Update test list
            TestListViewModel.Instance.LoadItems();
        }

        #endregion
    }
}

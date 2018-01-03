using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The view model for test editor (creator) page
    /// </summary>
    public class TestEditorViewModel : BaseViewModel
    {
        #region Commands

        /// <summary>
        /// The command to create new test
        /// </summary>
        public ICommand CreateNewTestCommand { get; private set; }

        /// <summary>
        /// The command to edit existing test
        /// </summary>
        public ICommand EditTestCommand { get; private set; }

        /// <summary>
        /// The command to create new criteria
        /// </summary>
        public ICommand CreateNewCriteriaCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestEditorViewModel()
        {
            // Create commands
            CreateNewTestCommand = new RelayCommand(() => ChangePage(ApplicationPage.TestEditorAddTest));
            EditTestCommand = new RelayCommand(() => ChangePage(ApplicationPage.TestEditorEditTest));
            CreateNewCriteriaCommand = new RelayCommand(() => ChangePage(ApplicationPage.TestEditorAddNewCriteria));
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Changes page to the specified one
        /// </summary>
        private void ChangePage(ApplicationPage page)
        {
            // Simply change page
            IoCServer.Application.GoToPage(page);
        }

        #endregion
    }
}

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

        #endregion

        #region Constructor

        public TestEditorEditTestViewModel()
        {
            // Create commands
            ChangePageCommand = new RelayCommand(ChangePage);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Changes page to add new test page with data from selected test in this page
        /// </summary>
        private void ChangePage()
        {
            // Check if user has selected any test
            if (CurrentTest == null)
                return;
        }

        #endregion
    }
}

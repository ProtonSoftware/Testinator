using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        #endregion

        #region Constructor

        public TestEditorViewModel()
        {
            // Create commands
            CreateNewTestCommand = new RelayCommand(CreateTest);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Changes page to new test creator page
        /// </summary>
        private void CreateTest()
        {
            // Simply change page
            IoCServer.Application.GoToPage(ApplicationPage.TestEditorAddTest);
        }

        #endregion
    }
}

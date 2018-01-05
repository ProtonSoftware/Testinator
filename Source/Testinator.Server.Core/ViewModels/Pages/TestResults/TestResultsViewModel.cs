using System;
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


        #endregion
        public ICommand cmd { get; set; }
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestResultsViewModel()
        {
            cmd = new RelayCommand(ds);
        }

        private void ds()
        {
            var vm = new ResultBoxDialogViewModel()
            {
                Message = "Hello there!",
                AcceptText = "Yes",
                CancelText = "No",
                Title = "Obi-wan kenobi",
            };
            IoCServer.UI.ShowMessage(vm);

        }

        #endregion
    }
}

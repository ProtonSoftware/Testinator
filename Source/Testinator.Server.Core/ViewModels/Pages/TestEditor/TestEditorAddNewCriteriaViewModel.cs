using System;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The view model for creating new criteria for future tests
    /// </summary>
    public class TestEditorAddNewCriteriaViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The criteria being created
        /// </summary>
        public Grading Criteria { get; set; } = new Grading();

        public string TopValueMarkA { get; set; } = "100";
        public string BottomValueMarkA { get; set; } = "96";
        public string TopValueMarkB { get; set; } = "95";
        public string BottomValueMarkB { get; set; } = "86";
        public string TopValueMarkC { get; set; } = "85";
        public string BottomValueMarkC { get; set; } = "70";
        public string TopValueMarkD { get; set; } = "69";
        public string BottomValueMarkD { get; set; } = "50";
        public string TopValueMarkE { get; set; } = "49";
        public string BottomValueMarkE { get; set; } = "30";
        public string TopValueMarkF { get; set; } = "29";
        public string BottomValueMarkF { get; set; } = "0";

        #endregion

        #region Commands

        /// <summary>
        /// The command to come back to main test editor page
        /// </summary>
        public ICommand ChangePageTestEditorCommand { get; private set; }

        /// <summary>
        /// The command to submit created criteria
        /// </summary>
        public ICommand SubmitCriteriaCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestEditorAddNewCriteriaViewModel()
        {
            // Create commands
            ChangePageTestEditorCommand = new RelayCommand(ChangePage);
            SubmitCriteriaCommand = new RelayCommand(SubmitCriteria);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Comes back to main test editor page
        /// </summary>
        private void ChangePage()
        {
            // Simply change page
            IoCServer.Application.GoToPage(ApplicationPage.TestEditor);
        }

        /// <summary>
        /// Submits the criteria to the list
        /// </summary>
        private void SubmitCriteria()
        {
            // Add every mark
            //Criteria.AddMark(Marks.A, top, bottom);
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Checks if input data is valid
        /// </summary>
        private void ValidateInputData()
        {

        }

        #endregion
    }
}

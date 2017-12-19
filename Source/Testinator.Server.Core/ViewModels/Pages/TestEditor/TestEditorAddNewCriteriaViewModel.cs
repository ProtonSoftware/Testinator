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

        /// <summary>
        /// Name of the criteria used for identifying
        /// </summary>
        public string CriteriaName { get; set; } = "";

        /// <summary>
        /// Indicates if mark A (the best grade) is counted (specific test can resign from this mark)
        /// </summary>
        public bool IsMarkACounted { get; set; } = true;

        /// <summary>
        /// Indicates if invalid data error should be shown
        /// </summary>
        public bool InvalidDataError { get; set; } = false;

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
        /// The command to select and load criteria from the list
        /// </summary>
        public ICommand SelectCriteriaCommand { get; private set; }

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
            SelectCriteriaCommand = new RelayParameterizedCommand((param) => SelectCriteria(param));
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
        /// Selects and loads the criteria from the list
        /// </summary>
        /// <param name="param">Name of the criteria</param>
        private void SelectCriteria(object param)
        {
            // Cast parameter to string
            string criteriaName = param.ToString();

            // Search for the criteria with that name
            foreach (var criteria in CriteriaListViewModel.Instance.Items)
            {
                // Check if name matches
                if (criteria.Name == criteriaName)
                {
                    // Get values to the view model's properties
                    if (criteria.IsMarkAIncluded)
                    {
                        //this.TopValueMarkA = criteria.Marks.A.TopLimitValue;
                    }
                }
            }
        }

        /// <summary>
        /// Submits the criteria to the list
        /// </summary>
        private void SubmitCriteria()
        {
            // Check if input data is correct
            if (!ValidateInputData())
                return;

            // Add every mark to the criteria object
            if (IsMarkACounted) Criteria.AddMark(Marks.A, Int32.Parse(TopValueMarkA), Int32.Parse(BottomValueMarkA));
            Criteria.AddMark(Marks.B, Int32.Parse(TopValueMarkB), Int32.Parse(BottomValueMarkB));
            Criteria.AddMark(Marks.C, Int32.Parse(TopValueMarkC), Int32.Parse(BottomValueMarkC));
            Criteria.AddMark(Marks.D, Int32.Parse(TopValueMarkD), Int32.Parse(BottomValueMarkD));
            Criteria.AddMark(Marks.E, Int32.Parse(TopValueMarkE), Int32.Parse(BottomValueMarkE));
            Criteria.AddMark(Marks.F, Int32.Parse(TopValueMarkF), Int32.Parse(BottomValueMarkF));

            // Send it to xml writer
            FileWriters.XmlWriter.Write(CriteriaName, this.Criteria);

            // Reload the criteria list to include newly created criteria
            CriteriaListViewModel.Instance.LoadItems();
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Checks if input data is valid
        /// </summary>
        private bool ValidateInputData()
        {
            try
            {
                // Parse every number value to integer
                int topMarkA = 0;
                int bottomMarkA = 0;
                if (IsMarkACounted)
                {
                    topMarkA = Int32.Parse(TopValueMarkA);
                    bottomMarkA = Int32.Parse(BottomValueMarkA);
                }
                int topMarkB = Int32.Parse(TopValueMarkB);
                int bottomMarkB = Int32.Parse(BottomValueMarkB);
                int topMarkC = Int32.Parse(TopValueMarkC);
                int bottomMarkC = Int32.Parse(BottomValueMarkC);
                int topMarkD = Int32.Parse(TopValueMarkD);
                int bottomMarkD = Int32.Parse(BottomValueMarkD);
                int topMarkE = Int32.Parse(TopValueMarkE);
                int bottomMarkE = Int32.Parse(BottomValueMarkE);
                int topMarkF = Int32.Parse(TopValueMarkF);
                int bottomMarkF = Int32.Parse(BottomValueMarkF);

                // Check if input data is in sequence
                if (IsMarkACounted)
                {
                    // The highest value must be 100
                    if (topMarkA != 100) throw new Exception();
                    if (bottomMarkA <= topMarkB) throw new Exception();
                }
                else
                {
                    // The highest value must be 100
                    if (topMarkB != 100) throw new Exception();
                }
                if (bottomMarkB <= topMarkC) throw new Exception();
                if (bottomMarkC <= topMarkD) throw new Exception();
                if (bottomMarkD <= topMarkE) throw new Exception();
                if (bottomMarkE <= topMarkF) throw new Exception();
                if (bottomMarkF != 0) throw new Exception();
            }
            catch
            {
                // Input data is invalid, show an error and return false
                InvalidDataError = true;
                return false;
            }

            // Everything works, return true
            return true;
        }

        #endregion
    }
}

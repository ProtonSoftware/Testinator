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
        public GradingPercentage Criteria { get; set; } = new GradingPercentage();

        /// <summary>
        /// Indicates if we are in editing existing criteria mode
        /// </summary>
        public bool EditingCriteriaMode { get; set; } = false;

        /// <summary>
        /// Name of the criteria which we are editing
        /// if remain unchanged - we replace criterias
        /// if user changes name - we create new one and delete old one
        /// </summary>
        public string EditingCriteriaOldName { get; set; }

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
        /// The command to edit criteria from the list
        /// </summary>
        public ICommand EditCriteriaCommand { get; private set; }

        /// <summary>
        /// The command to cancel editing criteria
        /// </summary>
        public ICommand CancelEditCriteriaCommand { get; private set; }

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
            EditCriteriaCommand = new RelayParameterizedCommand((param) => EditCriteria(param));
            CancelEditCriteriaCommand = new RelayCommand(CancelEdit);
            SubmitCriteriaCommand = new RelayCommand(SubmitCriteria);

            // Load the criteria list
            CriteriaListViewModel.Instance.LoadItems();
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
        /// Selects and loads the criteria from the list so user can edit it
        /// </summary>
        /// <param name="param">Name of the criteria</param>
        private void EditCriteria(object param)
        {
            // Indicate that we will be editing criteria instead of adding new one
            EditingCriteriaMode = true;

            // Cast parameter to string
            string criteriaName = param.ToString();

            // Search for the criteria with that name
            foreach (var criteria in CriteriaListViewModel.Instance.Items)
            {
                // Check if name matches
                if (criteria.Name == criteriaName)
                {
                    // Get values to the view model's properties
                    this.Criteria.Name = criteria.Name;
                    this.EditingCriteriaOldName = criteria.Name;
                    this.Criteria.IsMarkAIncluded = criteria.IsMarkAIncluded;
                    OnPropertyChanged(nameof(Criteria));

                    this.TopValueMarkA = criteria.MarkA.TopLimit.ToString();
                    this.BottomValueMarkA = criteria.MarkA.BottomLimit.ToString();
                    this.TopValueMarkB = criteria.MarkB.TopLimit.ToString();
                    this.BottomValueMarkB = criteria.MarkB.BottomLimit.ToString();
                    this.TopValueMarkC = criteria.MarkC.TopLimit.ToString();
                    this.BottomValueMarkC = criteria.MarkC.BottomLimit.ToString();
                    this.TopValueMarkD = criteria.MarkD.TopLimit.ToString();
                    this.BottomValueMarkD = criteria.MarkD.BottomLimit.ToString();
                    this.TopValueMarkE = criteria.MarkE.TopLimit.ToString();
                    this.BottomValueMarkE = criteria.MarkE.BottomLimit.ToString();
                    this.TopValueMarkF = criteria.MarkF.TopLimit.ToString();
                    this.BottomValueMarkF = criteria.MarkF.BottomLimit.ToString();

                    // Object found, no need to iterate more
                    break;
                }
            }
        }

        /// <summary>
        /// Cancels editing mode and leads to brand new criteria
        /// </summary>
        private void CancelEdit()
        {
            // Indicate that we are no longer editing criteria
            EditingCriteriaMode = false;
        }

        /// <summary>
        /// Submits the criteria to the list
        /// </summary>
        private void SubmitCriteria()
        {
            // Check if input data is correct
            if (!ValidateInputData())
            {
                InvalidDataError = true;
                return;
            }

            // Update values in grading object
            if (Criteria.IsMarkAIncluded) Criteria.UpdateMark(Marks.A, Int32.Parse(TopValueMarkA), Int32.Parse(BottomValueMarkA));
            Criteria.UpdateMark(Marks.B, Int32.Parse(TopValueMarkB), Int32.Parse(BottomValueMarkB));
            Criteria.UpdateMark(Marks.C, Int32.Parse(TopValueMarkC), Int32.Parse(BottomValueMarkC));
            Criteria.UpdateMark(Marks.D, Int32.Parse(TopValueMarkD), Int32.Parse(BottomValueMarkD));
            Criteria.UpdateMark(Marks.E, Int32.Parse(TopValueMarkE), Int32.Parse(BottomValueMarkE));
            Criteria.UpdateMark(Marks.F, Int32.Parse(TopValueMarkF), Int32.Parse(BottomValueMarkF));

            // Send it to xml writer and save it
            FileWriters.XmlWriter.SaveGrading(this.Criteria.Name, this.Criteria);

            // Check if we were editing existing one or not
            if (EditingCriteriaMode)
            {
                // If user has changed the name of the criteria, delete old one
                FileWriters.XmlWriter.DeleteFile(EditingCriteriaOldName);
            }

            // Get out of editing mode
            EditingCriteriaMode = false;
            EditingCriteriaOldName = string.Empty;

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
                if (Criteria.IsMarkAIncluded)
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
                if (Criteria.IsMarkAIncluded)
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

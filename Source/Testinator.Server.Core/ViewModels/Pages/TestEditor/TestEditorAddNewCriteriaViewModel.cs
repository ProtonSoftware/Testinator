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
        /// The criteria being created or edited
        /// </summary>
        public GradingPercentage Criteria { get; set; } = new GradingPercentage();

        /// <summary>
        /// Indicates if we are in editing existing criteria mode
        /// </summary>
        public bool EditingCriteriaMode { get; set; } = false;

        /// <summary>
        /// Name of the criteria which we are editing
        /// </summary>
        public string EditingCriteriaOldName { get; set; }
        
        /// <summary>
        /// Indicates if invalid data error should be shown
        /// </summary>
        public bool InvalidDataError { get; set; } = false;

        /// <summary>
        /// Name of the criteria being edited or created
        /// </summary>
        public string Name { get; set; }

        public string TopValueMarkA { get; set; }
        public string BottomValueMarkA { get; set; }
        public string TopValueMarkB { get; set; }
        public string BottomValueMarkB { get; set; }
        public string TopValueMarkC { get; set; }
        public string BottomValueMarkC { get; set; }
        public string TopValueMarkD { get; set; }
        public string BottomValueMarkD { get; set; }
        public string TopValueMarkE { get; set; }
        public string BottomValueMarkE { get; set; }
        public string TopValueMarkF { get; set; }
        public string BottomValueMarkF { get; set; }

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
        /// The command to either save changes or add new criteria
        /// </summary>
        public ICommand SubmitCriteriaCommand { get; private set; }

        /// <summary>
        /// The command to delete criteria
        /// </summary>
        public ICommand DeleteCriteriaCommand { get; private set; }

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
            DeleteCriteriaCommand = new RelayCommand(DeleteCommand);

            // Load the criteria list
            CriteriaListViewModel.Instance.LoadItems();

            // Load sample data
            LoadCriteria(new GradingPercentage());
            PropertyChanged += TestEditorAddNewCriteriaViewModel_PropertyChanged;
        }



        #endregion
        private void TestEditorAddNewCriteriaViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (EditingCriteriaMode)
            {
                if (e.PropertyName == nameof(Name) || e.PropertyName == nameof(TopValueMarkA) || e.PropertyName == nameof(BottomValueMarkA) || e.PropertyName == nameof(TopValueMarkB) ||
                    e.PropertyName == nameof(BottomValueMarkB) || e.PropertyName == nameof(TopValueMarkC) || e.PropertyName == nameof(BottomValueMarkC) || e.PropertyName == nameof(TopValueMarkD) ||
                    e.PropertyName == nameof(BottomValueMarkD) || e.PropertyName == nameof(TopValueMarkE) || e.PropertyName == nameof(BottomValueMarkE) || e.PropertyName == nameof(TopValueMarkF) || e.PropertyName == nameof(BottomValueMarkF))
                    CriteriaChanged = true;
            }
        }
        #region Command Methods

        /// <summary>
        /// Comes back to main test editor page
        /// </summary>
        private void ChangePage()
        {
            // If something is being edited show the warning...
            if (EditingCriteriaMode)
            {
                // TODO: show message box to ask the user if they want to save changes or not
            }
            else
                // Simply change page
                IoCServer.Application.GoToPage(ApplicationPage.TestEditor);

        }

        /// <summary>
        /// Selects and loads the criteria from the list so user can edit it
        /// </summary>
        /// <param name="param">Name of the criteria</param>
        private void EditCriteria(object param)
        {
            // Cast parameter to string
            string criteriaName = param.ToString();

            // If we are in editing mode
            if (EditingCriteriaMode)
            {
                // If the user clicked on the criteria that is now edited, do nothing
                if (criteriaName == EditingCriteriaOldName)
                    return;

                // If there are some unsaved changes
                if (CriteriaChanged)
                {
                    // TODO: Show the message box with the info that there are some unsaved changes
                    // Base on the resopond decide what to do,
                    // For now return without asking the user 

                    // If user wants to save changes do so and proceed to the remaining part of the function
                    //if (userRespond == true)
                    //   SubmitCriteria();
                }
            }

            // Mark all criteria unchecked
            CriteriaListViewModel.Instance.UncheckAll();

            // Hide any errors
            InvalidDataError = false;

            // Disable editing mode for items loading time, because it will fire the 
            // property changed event and the CriteriaChanged will be set, which we don't want for now
            EditingCriteriaMode = false;

            // Load the new data
            LoadCriteriaByName(criteriaName, true);

            // Enter editing mode again
            EditingCriteriaMode = true;
        }

        /// <summary>
        /// Cancels editing mode and leads to brand new criteria
        /// </summary>
        private void CancelEdit()
        {
            // Indicate that we are no longer editing criteria
            EditingCriteriaMode = false;

            // Mark all criteria unchecked
            CriteriaListViewModel.Instance.UncheckAll();

            // If something was changed...
            if (CriteriaChanged)
            {
                // Load the old criteria...
                LoadCriteriaByName(EditingCriteriaOldName, false);
            }
            else
            {
                // Load brand new object
                LoadCriteria(new GradingPercentage());
                Name = "";
            }
        }

        /// <summary>
        /// Submits the criteria or saves changes
        /// </summary>
        private void SubmitCriteria()
        {
            // Check if input data is correct
            if (!ValidateInputData())
            {
                InvalidDataError = true;
                return;
            }

            // Hide any errors
            InvalidDataError = false;

            // Update values in grading object
            if (Criteria.IsMarkAIncluded) Criteria.UpdateMark(Marks.A, Int32.Parse(TopValueMarkA), Int32.Parse(BottomValueMarkA));
            Criteria.UpdateMark(Marks.B, Int32.Parse(TopValueMarkB), Int32.Parse(BottomValueMarkB));
            Criteria.UpdateMark(Marks.C, Int32.Parse(TopValueMarkC), Int32.Parse(BottomValueMarkC));
            Criteria.UpdateMark(Marks.D, Int32.Parse(TopValueMarkD), Int32.Parse(BottomValueMarkD));
            Criteria.UpdateMark(Marks.E, Int32.Parse(TopValueMarkE), Int32.Parse(BottomValueMarkE));
            Criteria.UpdateMark(Marks.F, Int32.Parse(TopValueMarkF), Int32.Parse(BottomValueMarkF));

            // Send it to xml writer and save it
            FileWriters.XmlWriter.SaveGrading(this.Name, this.Criteria);

            // Check if we were editing existing one or not
            if (EditingCriteriaMode)
            {
                // If user has changed the name of the criteria, delete old one
                if (Name != EditingCriteriaOldName)
                    FileWriters.XmlWriter.DeleteFile(EditingCriteriaOldName);
            }

            // Get out of editing mode
            EditingCriteriaMode = false;

            // Mark all criteria unchecked
            CriteriaListViewModel.Instance.UncheckAll();

            // Reload the criteria list to include newly created criteria
            CriteriaListViewModel.Instance.LoadItems();

            // Load the viewmodel with the sample data
            LoadCriteria(new GradingPercentage());
            Name = "";
        }

        /// <summary>
        /// Deletes selected criteria
        /// </summary>
        private void DeleteCommand()
        {
            // Delete this criteria by old name because the user may have changed it meanwhile
            FileWriters.XmlWriter.DeleteFile(EditingCriteriaOldName);

            // Reload items
            CriteriaListViewModel.Instance.LoadItems();

            // Load brand new criteria
            LoadCriteria(new GradingPercentage());

            // Hide all errors and flags
            EditingCriteriaMode = false;
            InvalidDataError = false;
            CriteriaChanged = false;

            // Mark all criteria unchecked
            CriteriaListViewModel.Instance.UncheckAll();
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Sets up the view with the given criteria object
        /// </summary>
        /// <param name="criteria">The criteria that the view should be loaded with</param>
        private void LoadCriteria(GradingPercentage criteria)
        {
            // Hide errors and edit states
            CriteriaChanged = false;
            InvalidDataError = false;

            // Get values to the view model's properties
            this.Criteria = criteria;
            this.Name = criteria.Name;
            this.EditingCriteriaOldName = criteria.Name;

            this.Criteria.IsMarkAIncluded = criteria.IsMarkAIncluded;
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
        }

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

                if (string.IsNullOrWhiteSpace(Name)) throw new Exception();
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

        /// <summary>
        /// Loads criteria by the given name
        /// </summary>
        /// <param name="criteriaName">Name of the criteria to be loaded</param>
        /// <param name="ShouldBeMarkedSelected">Indicates if the newly loaded criteria should be marked selected on the list</param>
        private void LoadCriteriaByName(string criteriaName, bool ShouldBeMarkedSelected)
        {
            // Search for the criteria with that name
            foreach (var criteria in CriteriaListViewModel.Instance.Items)
            {

                // Check if name matches
                if (criteria.Grading.Name == criteriaName)
                {
                    // Update the view
                    LoadCriteria(criteria.Grading);

                    if(ShouldBeMarkedSelected)
                        criteria.IsSelected = true;     

                    // Object found, no need to iterate more
                    break;
                }
            }
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Makes sense in editing mode.
        /// If true: there are some changes to be saved,
        /// if false: nothing was edited and there's nothing to replace
        /// </summary>
        public bool CriteriaChanged { get; set; }

        #endregion
    }
}

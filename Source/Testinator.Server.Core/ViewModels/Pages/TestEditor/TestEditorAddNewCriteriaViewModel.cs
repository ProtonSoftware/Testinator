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

        /// <summary>
        /// Makes sense in editing mode.
        /// If true: there are some changes to be saved,
        /// if false: nothing was edited and there's nothing to replace
        /// </summary>
        public bool CriteriaChanged { get; set; }

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

            // Load default data
            LoadCriteria(new GradingPercentage());

            // Update the view
            PropertyChanged += TestEditorAddNewCriteriaViewModel_PropertyChanged;
        }

        #endregion

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
            var criteriaName = param.ToString();

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
            if (Criteria.IsMarkAIncluded) Criteria.UpdateMark(Marks.A, int.Parse(TopValueMarkA), int.Parse(BottomValueMarkA));
            Criteria.UpdateMark(Marks.B, int.Parse(TopValueMarkB), int.Parse(BottomValueMarkB));
            Criteria.UpdateMark(Marks.C, int.Parse(TopValueMarkC), int.Parse(BottomValueMarkC));
            Criteria.UpdateMark(Marks.D, int.Parse(TopValueMarkD), int.Parse(BottomValueMarkD));
            Criteria.UpdateMark(Marks.E, int.Parse(TopValueMarkE), int.Parse(BottomValueMarkE));
            Criteria.UpdateMark(Marks.F, int.Parse(TopValueMarkF), int.Parse(BottomValueMarkF));

            // Send it to xml writer and save it
            FileWriters.XmlWriter.WriteToFile(Name, Criteria);

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
                var topMarkA = 0;
                var bottomMarkA = 0;
                if (Criteria.IsMarkAIncluded)
                {
                    topMarkA = int.Parse(TopValueMarkA);
                    bottomMarkA = int.Parse(BottomValueMarkA);
                }
                var topMarkB = int.Parse(TopValueMarkB);
                var bottomMarkB = int.Parse(BottomValueMarkB);
                var topMarkC = int.Parse(TopValueMarkC);
                var bottomMarkC = int.Parse(BottomValueMarkC);
                var topMarkD = int.Parse(TopValueMarkD);
                var bottomMarkD = int.Parse(BottomValueMarkD);
                var topMarkE = int.Parse(TopValueMarkE);
                var bottomMarkE = int.Parse(BottomValueMarkE);
                var topMarkF = int.Parse(TopValueMarkF);
                var bottomMarkF = int.Parse(BottomValueMarkF);

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

        /// <summary>
        /// Fired when any property in this view model changes
        /// Used in editing mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestEditorAddNewCriteriaViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // If we are not in editing mode, don't bother doing anything
            if (!EditingCriteriaMode)
                return;

            // TODO: Fetch it to list and then check if e.PropertyName is in the list
            if (e.PropertyName == nameof(Name) || e.PropertyName == nameof(TopValueMarkA) || e.PropertyName == nameof(BottomValueMarkA) || e.PropertyName == nameof(TopValueMarkB) ||
                e.PropertyName == nameof(BottomValueMarkB) || e.PropertyName == nameof(TopValueMarkC) || e.PropertyName == nameof(BottomValueMarkC) || e.PropertyName == nameof(TopValueMarkD) ||
                e.PropertyName == nameof(BottomValueMarkD) || e.PropertyName == nameof(TopValueMarkE) || e.PropertyName == nameof(BottomValueMarkE) || e.PropertyName == nameof(TopValueMarkF) || e.PropertyName == nameof(BottomValueMarkF))
                CriteriaChanged = true;
        }

        #endregion
    }
}

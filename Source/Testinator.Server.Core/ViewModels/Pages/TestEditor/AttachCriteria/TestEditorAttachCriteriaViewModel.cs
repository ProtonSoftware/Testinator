using System;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The view model for the page where criteria is attached to the test
    /// </summary>
    public class TestEditorAttachCriteriaViewModel : BaseViewModel
    {
        #region Public Properties 

        /// <summary>
        /// Keeps the message of an error to show, if any error occured
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Current grading system attached to the test
        /// </summary>
        public GradingPercentage CurrentGrading { get; set; }

        /// <summary>
        /// Current grading system converted to the points based values
        /// </summary>
        public GradingPoints PointsGrading { get; set; }

        /// <summary>
        /// Indicates if user is editing criteria
        /// </summary>
        public bool IsCriteriaEditModeOn { get; set; } = false;

        public string EditingTopValueA { get; set; }
        public string EditingBottomValueA { get; set; }
        public string EditingTopValueB { get; set; }
        public string EditingBottomValueB { get; set; }
        public string EditingTopValueC { get; set; }
        public string EditingBottomValueC { get; set; }
        public string EditingTopValueD { get; set; }
        public string EditingBottomValueD { get; set; }
        public string EditingTopValueE { get; set; }
        public string EditingBottomValueE { get; set; }
        public string EditingTopValueF { get; set; }
        public string EditingBottomValueF { get; set; }

        #endregion

        #region Public Commands

        /// <summary>
        /// The command to go to the previous page
        /// </summary>
        public ICommand GoPreviousPageCommand { get; private set; }
        
        /// <summary>
        /// The command to submit criteria
        /// </summary>
        public ICommand SubmitCriteriaCommand { get; private set; }

        /// <summary>
        /// The command to manually edit the criteria
        /// </summary>
        public ICommand EditCommand { get; private set; }

        #endregion

        #region Command Methods

        /// <summary>
        /// Goes back to the previous page
        /// </summary>
        private void GoPreviousPage()
        {
            IoCServer.TestEditor.GoPreviousPage();
        }

        /// <summary>
        /// Submits current criteria
        /// </summary>
        private void SubmitCriteria()
        {
            try
            {
                // Check if input data is valid
                if (!ValidateInputData()) throw new Exception("Dane w polach są niepoprawne.");

                // Everything is ok, convert strings to integers and edit grading object
                if (PointsGrading.IsMarkAIncluded)
                {
                    PointsGrading.MarkA.TopLimit = int.Parse(EditingTopValueA);
                    PointsGrading.MarkA.BottomLimit = int.Parse(EditingBottomValueA);
                }
                PointsGrading.MarkB.TopLimit = int.Parse(EditingTopValueB);
                PointsGrading.MarkB.BottomLimit = int.Parse(EditingBottomValueB);
                PointsGrading.MarkC.TopLimit = int.Parse(EditingTopValueC);
                PointsGrading.MarkC.BottomLimit = int.Parse(EditingBottomValueC);
                PointsGrading.MarkD.TopLimit = int.Parse(EditingTopValueD);
                PointsGrading.MarkD.BottomLimit = int.Parse(EditingBottomValueD);
                PointsGrading.MarkE.TopLimit = int.Parse(EditingTopValueE);
                PointsGrading.MarkE.BottomLimit = int.Parse(EditingBottomValueE);
                PointsGrading.MarkF.TopLimit = int.Parse(EditingTopValueF);
                PointsGrading.MarkF.BottomLimit = int.Parse(EditingBottomValueF);

                // Get away from editing mode
                IsCriteriaEditModeOn = false;
                ErrorMessage = string.Empty;
                OnPropertyChanged(nameof(PointsGrading));
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return;
            }
            try
            {
                IoCServer.TestEditor.Builder.AddGrading(PointsGrading);
            }
            catch (Exception ex)
            {
                IoCServer.UI.ShowMessage(new MessageBoxDialogViewModel()
                {
                    Title = "Test editor",
                    Message = $"Nieznany bład. {ex.Message}",
                });

                return;
            }

            IoCServer.TestEditor.GoNextPhase();
        }

        /// <summary>
        /// Fired when an item is selected from criteria list
        /// </summary>
        /// <param name="newGrading">Item that has been clicked</param>
        private void CriteriaItemSelected(GradingPercentage newGrading)
        {
            PointsGrading = newGrading.ToPoints(IoCServer.TestEditor.Builder.CurrentPointScore);
            IsCriteriaEditModeOn = false;
            ErrorMessage = "";
            MatchPropertiesWithCriteria();
        }

        /// <summary>
        /// Begins editing current criteria
        /// </summary>
        private void BeginEdit()
        {
            IsCriteriaEditModeOn = true;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestEditorAttachCriteriaViewModel()
        {
            // Create commands
            GoPreviousPageCommand = new RelayCommand(GoPreviousPage);
            SubmitCriteriaCommand = new RelayCommand(SubmitCriteria);
            EditCommand = new RelayCommand(BeginEdit);

            CriteriaListViewModel.Instance.LoadItems();
            CriteriaListViewModel.Instance.ShouldSelectIndicatorBeVisible = false;
            CriteriaListViewModel.Instance.ItemSelected += CriteriaItemSelected;

            PointsGrading =  new GradingPercentage().ToPoints(IoCServer.TestEditor.CurrentPointScore);

            MatchPropertiesWithCriteria();
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Checks if criteria input data is valid
        /// </summary>
        private bool ValidateInputData()
        {
            try
            {
                // Parse every number value to integer
                var topMarkA = 0;
                var bottomMarkA = 0;
                if (PointsGrading.IsMarkAIncluded)
                {
                    topMarkA = int.Parse(EditingTopValueA);
                    bottomMarkA = int.Parse(EditingBottomValueA);
                }
                var topMarkB = int.Parse(EditingTopValueB);
                var bottomMarkB = int.Parse(EditingBottomValueB);
                var topMarkC = int.Parse(EditingTopValueC);
                var bottomMarkC = int.Parse(EditingBottomValueC);
                var topMarkD = int.Parse(EditingTopValueD);
                var bottomMarkD = int.Parse(EditingBottomValueD);
                var topMarkE = int.Parse(EditingTopValueE);
                var bottomMarkE = int.Parse(EditingBottomValueE);
                var topMarkF = int.Parse(EditingTopValueF);
                var bottomMarkF = int.Parse(EditingBottomValueF);

                // Check if input data is in sequence
                if (PointsGrading.IsMarkAIncluded)
                {
                    // The highest value must be test's max points
                    if (topMarkA != IoCServer.TestEditor.Builder.CurrentPointScore) throw new Exception();
                    if (bottomMarkA <= topMarkB) throw new Exception();
                }
                else
                {
                    // The highest value must be test's max points
                    if (topMarkB != IoCServer.TestEditor.Builder.CurrentPointScore) throw new Exception();
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
                ErrorMessage = "Dane w polach są niepoprawne.";
                return false;
            }

            // Everything works, return true
            return true;
        }

        /// <summary>
        /// Matches the string properties in this VM with actual PointsGrading values
        /// </summary>
        private void MatchPropertiesWithCriteria()
        {
            if (PointsGrading == null)
                return;

            // Simply take values from PointsGrading to every property
            EditingTopValueA = PointsGrading.MarkA.TopLimit.ToString();
            EditingBottomValueA = PointsGrading.MarkA.BottomLimit.ToString();
            EditingTopValueB = PointsGrading.MarkB.TopLimit.ToString();
            EditingBottomValueB = PointsGrading.MarkB.BottomLimit.ToString();
            EditingTopValueC = PointsGrading.MarkC.TopLimit.ToString();
            EditingBottomValueC = PointsGrading.MarkC.BottomLimit.ToString();
            EditingTopValueD = PointsGrading.MarkD.TopLimit.ToString();
            EditingBottomValueD = PointsGrading.MarkD.BottomLimit.ToString();
            EditingTopValueE = PointsGrading.MarkE.TopLimit.ToString();
            EditingBottomValueE = PointsGrading.MarkE.BottomLimit.ToString();
            EditingTopValueF = PointsGrading.MarkF.TopLimit.ToString();
            EditingBottomValueF = PointsGrading.MarkF.BottomLimit.ToString();
        }

        #endregion

        #region Cleanup

        /// <summary>
        /// Dispose this viewmodel
        /// </summary>
        public override void Dispose()
        {
            CriteriaListViewModel.Instance.ItemSelected -= CriteriaItemSelected;
        }

        #endregion
    }
}

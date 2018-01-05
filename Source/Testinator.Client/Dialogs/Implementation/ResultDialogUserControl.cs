using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Client
{
    /// <summary>
    /// The class for ResultDialog box with a view model for it
    /// </summary>
    public class ResultDialogUserControl : BaseDialogUserControl
    {
        #region Private Members

        /// <summary>
        /// The view model of this result box
        /// </summary>
        private ResultBoxDialogViewModel mResultVM;

        #endregion

        #region Public Properties

        /// <summary>
        /// The view model of this result box
        /// Get - returns the view model itself
        /// Set - sets the value and data context of the <see cref="DialogWindow"/>
        /// </summary>
        public ResultBoxDialogViewModel ResultVM
        {
            get => mResultVM;
            set
            {
                mResultVM = value;
                DataContext = value;
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// The command to close this dialog and returns user's response value
        /// </summary>
        public ICommand CloseWithResponseCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ResultDialogUserControl()
        {
            // Create commands
            CloseWithResponseCommand = new RelayParameterizedCommand((param) => 
            {
                // Cast the parameter
                var userResponse = param.ToString();

                // Save the user's answer
                ResultVM.UserResponse = userResponse == "True" ? true : false;

                // Close the dialog window
                mDialogWindow.Close();
            });
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Sets the view model of this ResultDialog box
        /// </summary>
        /// <param name="viewModel">The view model to set</param>
        public override void SetDialogViewModel(ResultBoxDialogViewModel viewModel)
        {
            // Simply set the view model to the property
            ResultVM = viewModel;
        }

        #endregion
    }
}

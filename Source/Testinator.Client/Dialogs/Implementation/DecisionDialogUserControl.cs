using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Client
{
    /// <summary>
    /// The class for DecisionDialog box with a view model for it
    /// </summary>
    public class DecisionDialogUserControl : BaseDialogUserControl
    {
        #region Commands

        /// <summary>
        /// The command to close this dialog and return user's response value
        /// </summary>
        public ICommand CloseWithResponseCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public DecisionDialogUserControl()
        {
            // Create commands
            CloseWithResponseCommand = new RelayParameterizedCommand((param) =>
            {
                // Cast the parameter
                var userResponse = param.ToString();

                // Save the user's answer
                (ContentViewModel as DecisionDialogViewModel).UserResponse = userResponse == "True" ? true : false;

                // Close the dialog window
                mDialogWindow.Close();
            });
        }

        #endregion
    }
}

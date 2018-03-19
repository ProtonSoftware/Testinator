using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server
{
    /// <summary>
    /// The class for AddLatecomersDialog box with a view model for it
    /// </summary>
    public class AddLatecomersDialogUserControl : BaseDialogUserControl
    {
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
        public AddLatecomersDialogUserControl()
        {
            // Create commands
            CloseWithResponseCommand = new RelayParameterizedCommand((param) =>
            {
                // Cast the parameter
                var userResponse = param.ToString();

                // Save the user's answer
                if (userResponse == "True")
                    (ContentViewModel as AddLatecomersDialogViewModel).AcceptAndClose();
                else
                    (ContentViewModel as AddLatecomersDialogViewModel).CancelAndClose();

                // Close the dialog window
                mDialogWindow.Close();
            });
        }

        #endregion
    }
}

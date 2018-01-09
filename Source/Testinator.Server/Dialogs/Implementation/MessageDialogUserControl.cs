using System.Threading.Tasks;
using System.Windows.Input;
using Testinator.Core;
using Testinator.Server.Core;

namespace Testinator.Server
{
    /// <summary>
    /// The class for MessageDialog box with a view model for it
    /// </summary>
    public class MessageDialogUserControl : BaseDialogUserControl
    {
        #region Private Members

        /// <summary>
        /// The view model of this message box
        /// </summary>
        private MessageBoxDialogViewModel mMessageVM;

        #endregion

        #region Public Properties

        /// <summary>
        /// The view model of this message box
        /// Get - returns the view model itself
        /// Set - sets the value and data context of the <see cref="DialogWindow"/>
        /// </summary>
        public MessageBoxDialogViewModel MessageVM
        {
            get => mMessageVM;
            set
            {
                mMessageVM = value;
                DataContext = value;
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// The command to close this dialog
        /// </summary>
        public ICommand CloseCommand { get; private set; }

        #endregion

        #region Constructor

        public MessageDialogUserControl()
        {
            // Create commands
            CloseCommand = new RelayCommand(() => mDialogWindow.Close());
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Sets the view model of this MessageDialog box
        /// </summary>
        /// <param name="viewModel">The view model to set</param>
        public override void SetDialogViewModel(MessageBoxDialogViewModel viewModel)
        {
            // Simply set the view model to the property
            MessageVM = viewModel;
        }

        public new Task ShowDialog<T>(T viewmodel) where T : BaseDialogViewModel
        {
            // Check if ApplicationSettings allow showing this type of dialog box
            if (!IoCServer.Settings.AreInformationMessageBoxesAllowed)
                return Task.Delay(1);

            // Now we can show the message
            base.ShowDialog<T>(viewmodel);

            return Task.Delay(1);
        }

        #endregion
    }
}

using Testinator.Core;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Testinator.Server.Core;

namespace Testinator.Server
{
    /// <summary>
    /// The base class for any content that is being used inside of a <see cref="DialogWindow"/>
    /// </summary>
    public class BaseDialogUserControl : UserControl
    {
        #region Private Members

        /// <summary>
        /// The dialog window we will be contained within
        /// </summary>
        private DialogWindow mDialogWindow;

        #endregion

        private ResultBoxDialogViewModel mResultVM;

        public ResultBoxDialogViewModel ResultVM
        {
            get => mResultVM;
            set
            {
                mResultVM = value;

                DataContext = value;
            }
        }

        private MessageBoxDialogViewModel mMessageVM;

        public MessageBoxDialogViewModel MessageVM
        {
            get => mMessageVM;
            set
            {
                mMessageVM = value;

                DataContext = value;
            }
        }


        #region Commands

        /// <summary>
        /// Closes this dialog
        /// </summary>
        public ICommand CloseCommand { get; private set; }

        /// <summary>
        /// Closes this dialog and returns user's agree value
        /// </summary>
        public ICommand AgreeCommand { get; private set; }

        /// <summary>
        /// Closes this dialog and return user's cancel value
        /// </summary>
        public ICommand CancelCommand { get; private set; }

        #endregion

        #region Public Properties

        /// <summary>
        /// The minimum width of this dialog
        /// </summary>
        public int WindowMinimumWidth { get; set; } = 250;

        /// <summary>
        /// The minimum height of this dialog
        /// </summary>
        public int WindowMinimumHeight { get; set; } = 100;

        /// <summary>
        /// The height of the title bar
        /// </summary>
        public int TitleHeight { get; set; } = 30;

        /// <summary>
        /// The title for this dialog
        /// </summary>
        public string Title { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BaseDialogUserControl()
        {
            // Dont do anything in design mode
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
            
            // Create a new dialog window
            mDialogWindow = new DialogWindow();
            mDialogWindow.ViewModel = new DialogWindowViewModel(mDialogWindow);

            // Create commands
            CloseCommand = new RelayCommand(() => mDialogWindow.Close());
            AgreeCommand = new RelayCommand(() => 
            {
                // Save the user's answer
                ResultVM.HasUserAgreed = true;

                // Close the dialog window
                mDialogWindow.Close();
            });
            CancelCommand = new RelayCommand(() =>
            {
                // Save the user's answer
                ResultVM.HasUserAgreed = false;

                // Close the dialog window
                mDialogWindow.Close();
            });
        }

        #endregion

        #region Public Dialog Show Methods

        /// <summary>
        /// Displays a single message box to the user
        /// </summary>
        /// <param name="viewModel">The view model</param>
        /// <typeparam name="T">The view model type for this control</typeparam>
        /// <returns></returns>
        public Task ShowDialog<T>(T viewModel)
            where T : BaseDialogViewModel
        {
            // Create a task to await the dialog closing
            var tcs = new TaskCompletionSource<bool>();

            // Run on UI thread
            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    // Match controls expected sizes to the dialog windows view model
                    mDialogWindow.ViewModel.WindowMinimumWidth = WindowMinimumWidth;
                    mDialogWindow.ViewModel.WindowMinimumHeight = WindowMinimumHeight;
                    mDialogWindow.ViewModel.TitleHeight = TitleHeight;
                    mDialogWindow.ViewModel.Title = string.IsNullOrEmpty(viewModel.Title) ? Title : viewModel.Title;

                    // Set this control to the dialog window content
                    mDialogWindow.ViewModel.Content = this;

                    // Setup this controls data context binding to the view model
                    if (viewModel is ResultBoxDialogViewModel) ResultVM = viewModel as ResultBoxDialogViewModel;
                    else if (viewModel is MessageBoxDialogViewModel) MessageVM = viewModel as MessageBoxDialogViewModel;

                    // Show in the center of the parent
                    mDialogWindow.Owner = Application.Current.MainWindow;
                    mDialogWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                    // Show dialog
                    mDialogWindow.ShowDialog();
                }
                finally
                {
                    // Let caller know we finished
                    tcs.TrySetResult(true);
                }
            });

            return tcs.Task;
        }

        #endregion
    }
}
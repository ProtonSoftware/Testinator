using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Testinator.Server.Core;

namespace Testinator.Server
{
    /// <summary>
    /// The base class for any content that is being used inside of a <see cref="DialogWindow"/>
    /// </summary>
    public abstract class BaseDialogUserControl : UserControl
    {
        #region Protected Members

        /// <summary>
        /// The dialog window we will be contained within
        /// </summary>
        protected DialogWindow mDialogWindow;

        #endregion

        #region Public Properties

        /// <summary>
        /// The minimum width of this dialog
        /// </summary>
        public int WindowMinimumWidth { get; set; } = 500;

        /// <summary>
        /// The minimum height of this dialog
        /// </summary>
        public int WindowMinimumHeight { get; set; } = 200;

        /// <summary>
        /// The height of the title bar
        /// </summary>
        public int TitleHeight { get; set; } = 36;

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

                    // Setup this controls data context binding to the specified view model depends on its type
                    if (viewModel is ResultBoxDialogViewModel)
                        SetDialogViewModel(viewModel as ResultBoxDialogViewModel);
                    else if (viewModel is MessageBoxDialogViewModel)
                        SetDialogViewModel(viewModel as MessageBoxDialogViewModel);

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

        #region Override Methods

        /// <summary>
        /// Sets the view model of a ResultDialog box
        /// </summary>
        /// <param name="viewModel">The view model to set</param>
        public virtual void SetDialogViewModel(ResultBoxDialogViewModel viewModel) { }

        /// <summary>
        /// Sets the view model of a MessageDialog box
        /// </summary>
        /// <param name="viewModel">The view model to set</param>
        public virtual void SetDialogViewModel(MessageBoxDialogViewModel viewModel) { }

        #endregion
    }
}
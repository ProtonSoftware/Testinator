using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Testinator.Core;

namespace Testinator.Client
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

        /// <summary>
        /// The view model for content of this dialog
        /// </summary>
        private BaseDialogViewModel mDialogViewModel;

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

        /// <summary>
        /// The view model for content of this messagebox
        /// Get - returns the view model itself
        /// Set - sets data context of this viewmodel and stores it
        /// </summary>
        public BaseDialogViewModel ContentViewModel
        {
            get => mDialogViewModel;
            set
            {
                mDialogViewModel = value;
                DataContext = value;
            }
        }

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
        }

        #endregion

        #region Public Dialog Show Methods

        /// <summary>
        /// Displays a single message box to the user
        /// </summary>
        /// <param name="viewModel">The view model</param>
        /// <typeparam name="T">The view model type for this control</typeparam>
        /// <returns></returns>
        public virtual Task ShowDialog<T>(T viewModel)
            where T : BaseDialogViewModel
        {
            // Create a task to await the dialog closing
            var tcs = new TaskCompletionSource<bool>();

            // Run on UI thread
            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    // Set the viewmodel for the window
                    mDialogWindow.DataContext = new DialogWindowViewModel(mDialogWindow)
                    {
                        // Match controls expected sizes to the dialog windows view model
                        WindowMinimumWidth = WindowMinimumWidth,
                        WindowMinimumHeight = WindowMinimumHeight,
                        TitleHeight = TitleHeight,
                        Title = string.IsNullOrEmpty(viewModel.Title) ? Title : viewModel.Title,
                        Content = this,
                    };

                    // Set the viewmodel passed in as a viewmodel for the content of this dialog
                    ContentViewModel = viewModel;

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
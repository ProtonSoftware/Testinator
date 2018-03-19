using System.Windows;
using System.Windows.Input;
using System.Windows.Shell;
using Testinator.Client.Core;
using Testinator.Core;
using Testinator.UICore;

namespace Testinator.Client
{
    /// <summary>
    /// The View Model for the custom flat window
    /// </summary>
    public class WindowViewModel : BaseViewModel
    {
        #region Private Member

        /// <summary>
        /// The window this view model controls
        /// </summary>
        private Window mWindow;

        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        private int mOuterMarginSize = 10;

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        private int mWindowRadius = 3;

        /// <summary>
        /// The last known dock position
        /// </summary>
        private WindowDockPosition mDockPosition = WindowDockPosition.Undocked;

        /// <summary>
        /// Indicates if this window is in fullscreen mode
        /// </summary>
        private bool mFullscreenMode = false;

        #endregion

        #region Public Properties

        /// <summary>
        /// The smallest width the window can go to
        /// </summary>
        public double WindowMinimumWidth { get; set; } = 800;

        /// <summary>
        /// The smallest height the window can go to
        /// </summary>
        public double WindowMinimumHeight { get; set; } = 720;

        /// <summary>
        /// True if the window should be borderless because it is docked or maximized
        /// </summary>
        public bool Borderless => (mWindow.WindowState == WindowState.Maximized || mDockPosition != WindowDockPosition.Undocked);

        /// <summary>
        /// The size of the resize border around the window
        /// </summary>
        public int ResizeBorder => Borderless ? 0 : 4;

        /// <summary>
        /// The size of the resize border around the window, taking into account the outer margin
        /// </summary>
        public Thickness ResizeBorderThickness => new Thickness(ResizeBorder + OuterMarginSize);

        /// <summary>
        /// The padding of the inner content of the main window
        /// </summary>
        public Thickness InnerContentPadding { get; set; } = new Thickness(0);

        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        public int OuterMarginSize
        {
            // If it is maximized or docked, no border
            get => Borderless ? 0 : mOuterMarginSize;
            set => mOuterMarginSize = value;
        }

        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        public Thickness OuterMarginSizeThickness => new Thickness(OuterMarginSize);

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        public int WindowRadius
        {
            // If it is maximized or docked, no border
            get => Borderless ? 0 : mWindowRadius;
            set => mWindowRadius = value;
        }

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        public CornerRadius WindowCornerRadius => new CornerRadius(WindowRadius);

        /// <summary>
        /// The height of the title bar / caption of the window
        /// </summary>
        public int TitleHeight { get; set; } = 32;

        /// <summary>
        /// The height of the title bar / caption of the window converted to the grid length
        /// </summary>
        public GridLength TitleHeightGridLength => new GridLength(TitleHeight + ResizeBorder);

        #endregion

        #region Commands

        /// <summary>
        /// The command to minimize the window
        /// </summary>
        public ICommand MinimizeCommand { get; set; }

        /// <summary>
        /// The command to maximize the window
        /// </summary>
        public ICommand MaximizeCommand { get; set; }

        /// <summary>
        /// The command to close the window
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// The command to show the system menu of the window
        /// </summary>
        public ICommand MenuCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public WindowViewModel(Window window)
        {
            mWindow = window;

            // Listen out for the window resizing
            mWindow.StateChanged += (sender, e) =>
            {
                // Fire off events for all properties that are affected by a resize
                WindowResized();
            };

            // Create commands
            MinimizeCommand = new RelayCommand(() =>
            {
                // This action is allowed only outside of a fullscreen mode
                if (!mFullscreenMode)
                    mWindow.WindowState = WindowState.Minimized;
            });
            MaximizeCommand = new RelayCommand(() => 
            {
                // This action is allowed only outside of a fullscreen mode
                if (!mFullscreenMode)
                    mWindow.WindowState ^= WindowState.Maximized;
            }); 
            CloseCommand = new RelayCommand(() =>
            {
                // Check if any test is already in progress
                if (IoCClient.TestHost.IsTestInProgress)
                {
                    // Show warning to the user and do not close the app
                    IoCClient.UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Title = "Ostrzeżenie",
                        Message = "Aplikacja nie może zostać zamknięta, gdy test jest w trakcie.",
                        OkText = "OK"
                    });
                }
                else
                {
                    // Ask the user, if he is certain he wants to close the app
                    var vm = new DecisionDialogViewModel
                    {
                        Title = "Zamykanie aplikacji",
                        Message = "Czy na pewno chcesz wyłączyć aplikację?",
                        AcceptText = "Tak",
                        CancelText = "Nie"
                    };
                    IoCClient.UI.ShowMessage(vm);
                    
                    if (vm.UserResponse)
                        mWindow.Close();
                }
            }); 
            MenuCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(mWindow, GetMousePosition()));

            // Fix window resize issue
            var resizer = new WindowResizer(mWindow);

            // Listen out for dock changes
            resizer.WindowDockChanged += (dock) =>
            {
                // Store last position
                mDockPosition = dock;

                // Fire off resize events
                WindowResized();
            };

            // Listen out for full screen mode requests
            IoCClient.TestHost.FullScreenModeOn += TestHost_FullScreenModeOn;
            IoCClient.TestHost.FullScreenModeOff += TestHost_FullScreenModeOff;
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Gets the current mouse position on the screen
        /// </summary>
        /// <returns></returns>
        private Point GetMousePosition()
        {
            // Position of the mouse relative to the window
            var position = Mouse.GetPosition(mWindow);

            // Add the window position so its a "ToScreen"
            return new Point(position.X + mWindow.Left, position.Y + mWindow.Top);
        }

        /// <summary>
        /// If the window resizes to a special position (docked or maximized)
        /// this will update all required property change events to set the borders and radius values
        /// </summary>
        private void WindowResized()
        {
            // Fire off events for all properties that are affected by a resize
            OnPropertyChanged(nameof(Borderless));
            OnPropertyChanged(nameof(ResizeBorderThickness));
            OnPropertyChanged(nameof(OuterMarginSize));
            OnPropertyChanged(nameof(OuterMarginSizeThickness));
            OnPropertyChanged(nameof(WindowRadius));
            OnPropertyChanged(nameof(WindowCornerRadius));
        }

        /// <summary>
        /// Runs a full screen mode
        /// </summary>
        private void TestHost_FullScreenModeOn()
        {
            // Indicate that this window changes to full screen mode
            mFullscreenMode = true;

            // Make sure we are on UIThread
            Application.Current.Dispatcher.Invoke(() =>
            {
                // Prevent alt tabs etc.
                (mWindow as MainWindow).PreventUserEscapeActions();

                // Go to real "full screen mode"
                WindowChrome.SetWindowChrome(mWindow, new WindowChrome
                {
                    CaptionHeight = 0,
                    ResizeBorderThickness = new Thickness(0)
                });
                mWindow.Topmost = true;
                mWindow.WindowStyle = WindowStyle.None;
                mWindow.ResizeMode = ResizeMode.NoResize;
                mWindow.WindowState = WindowState.Maximized;
            });
        }

        /// <summary>
        /// Disables the full screen mode
        /// </summary>
        private void TestHost_FullScreenModeOff()
        {
            // Indicate that this window changes to normal mode
            mFullscreenMode = false;

            // Make sure we are on UIThread
            Application.Current.Dispatcher.Invoke(() =>
            {
                // Allow alt tabs etc.
                (mWindow as MainWindow).AllowUserActions();

                // Come back to initial window state
                WindowChrome.SetWindowChrome(mWindow, new WindowChrome
                {
                    CaptionHeight = TitleHeight,
                    ResizeBorderThickness = ResizeBorderThickness,
                    CornerRadius = new CornerRadius(0),
                    GlassFrameThickness = new Thickness(0)
                });
                mWindow.Topmost = false;
                mWindow.WindowStyle = WindowStyle.None;
                mWindow.ResizeMode = ResizeMode.CanResize;
                mWindow.WindowState = WindowState.Normal;
            });
        }

        #endregion
    }
}
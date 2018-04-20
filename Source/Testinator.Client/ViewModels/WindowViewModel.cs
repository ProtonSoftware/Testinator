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

        /// <summary>
        /// Minimum width of the window
        /// </summary>
        private double mWindowMinimumWidth = 800;

        /// <summary>
        /// Minimum height of the window
        /// </summary>
        private double mWindowMinimumHeight = 720;

        /// <summary>
        /// Login screen width
        /// </summary>
        private const int LoginScreenWidth = 750;

        /// <summary>
        /// Login screen height
        /// </summary>
        private const int LoginScreenHeight = 500;

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
        public Thickness OuterMarginSizeThickness => TitleBarVisible ? new Thickness(OuterMarginSize) : new Thickness(0, 10, 0, 0);

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        public int WindowRadius
        {
            // If it is maximized or docked, or no title bar no corner radius
            get => (Borderless || !TitleBarVisible ) ? 0 : mWindowRadius;
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
        /// The value of title height to bind to in xaml
        /// </summary>
        public int TitleHeightXaml => TitleBarVisible ? TitleHeight : 0;

        /// <summary>
        /// The height of area that can move window
        /// </summary>
        public int CaptionHeight => TitleBarVisible ? TitleHeight : 10 + OuterMarginSize;

        /// <summary>
        /// Indicates if window title bar should be visible
        /// </summary>
        public bool TitleBarVisible { get; set; } = true;

        /// <summary>
        /// The height of the title bar / caption of the window converted to the grid length
        /// </summary>
        public GridLength TitleHeightGridLength => new GridLength(TitleHeight + ResizeBorder);

        #endregion

        #region Public Methods

        /// <summary>
        /// Runs a full screen mode
        /// </summary>
        public void FullScreenModeOn()
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
        public void FullScreenModeOff()
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

        /// <summary>
        /// Enables small application format (used in login page)
        /// </summary>
        public void EnableSmallFormat()
        {
            // Hide title bar
            TitleBarVisible = false;

            // Set no resize mode
            mWindow.ResizeMode = ResizeMode.NoResize;

            // Store current minimum values
            mWindowMinimumHeight = WindowMinimumHeight;
            mWindowMinimumWidth = WindowMinimumWidth;

            // Set login scree dimensions as minimum values for now so the window can go smaller than defined
            WindowMinimumHeight = LoginScreenHeight;
            WindowMinimumWidth = LoginScreenWidth;

            // Set current dimensions
            mWindow.Height = LoginScreenHeight;
            mWindow.Width = LoginScreenWidth;
        }

        /// <summary>
        /// Disables small application format (used in login page)
        /// </summary>
        public void DisableSmallFormat()
        {
            // Show title bar
            TitleBarVisible = true;

            // Set resize mode
            mWindow.ResizeMode = ResizeMode.CanResize;

            // Restore minimum dimenstions
            WindowMinimumHeight = mWindowMinimumHeight;
            WindowMinimumWidth = mWindowMinimumWidth;

            // Set current dimensions to the minimum dimensions
            mWindow.Height = WindowMinimumHeight;
            mWindow.Width = WindowMinimumWidth;
        }

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

        #endregion

    }
}
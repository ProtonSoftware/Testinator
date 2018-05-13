using System;
using System.Reflection;

namespace Testinator.Client.Core
{
    /// <summary>
    /// The application state as a view model
    /// </summary>
    public class ApplicationViewModel : PageHostViewModel
    {
        #region Public Properties

        /// <summary>
        /// Handles network communication in the application
        /// </summary>
        public ClientNetwork Network { get; set; } = new ClientNetwork();

        /// <summary>
        /// Current version of the application
        /// </summary>
        public Version Version { get; private set; }

        /// <summary>
        /// Indicates how much time is left 
        /// </summary>
        public TimeSpan TimeLeft => IoCClient.TestHost.TimeLeft;

        #endregion

        #region Public Events

        /// <summary>
        /// Fired when main application is closing, so some operation may trigger this event 
        /// and prepare for closing
        /// </summary>
        public event Action OnAppClosing = () => { };

        #endregion

        #region Constructor

        /// <summary>
        /// Default construcotr
        /// </summary>
        public ApplicationViewModel()
        {
            // Get the current version from assebly
            var assebly = Assembly.LoadFrom("Testinator.Client.Core.dll");
            Version = assebly.GetName().Version;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns to the login page if there is no connection to the sevrer
        /// or to the waitingForTestPage if it is still connected
        /// </summary>
        public void ReturnMainScreen()
        {
            if (Network.IsConnected)
            {
                IoCClient.UI.DispatcherThreadAction(() => IoCClient.Application.GoToPage(ApplicationPage.WaitingForTest));
            }
            else
                IoCClient.UI.DispatcherThreadAction(() => IoCClient.Application.GoToPage(ApplicationPage.Login));
        }

        /// <summary>
        /// Closes the application
        /// </summary>
        public void Close()
        {
            OnAppClosing.Invoke();
        }

        /// <summary>
        /// Fired when application page changes
        /// </summary>
        /// <param name="newPage">The new page</param>
        public override void OnPageChange(ApplicationPage newPage)
        {
            // In both cases we need small format
            if (newPage == ApplicationPage.Login || newPage == ApplicationPage.WaitingForTest)
                IoCClient.UI.EnableSmallApplicationView();

            // NOTE: If there was only 'else' here it would cause useless calls to UIManager to disable login screen view 
            //       that has already been disabled
            else if (newPage > ApplicationPage.WaitingForTest)
                IoCClient.UI.DisableSmallApplicationView();
        }

        #endregion
    }
}

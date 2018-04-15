using System;
using System.Reflection;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The application state as a view model
    /// </summary>
    public class ApplicationViewModel : PageHostViewModel
    {
        #region Public Properties

        /// <summary>
        /// The language of this application
        /// </summary>
        public string ApplicationLanguage { get; set; } = "pl-PL";

        /// <summary>
        /// Current version of the application
        /// </summary>
        public Version Version { get; private set; }

        /// <summary>
        /// Show the side menu only if we are not on login page
        /// </summary>
        public bool SideMenuVisible => CurrentPage != ApplicationPage.Login;

        /// <summary>
        /// The current subpage of the BeginTestPage
        /// </summary>
        public ApplicationPage CurrentBeginTestPage { get; private set; } = ApplicationPage.BeginTestInitial;

        #endregion

        #region Public Events

        /// <summary>
        /// Fired when main application is closing, so some operation may trigger this event 
        /// and prepare for closing
        /// </summary>
        public event Action Closing = () => { };

        #endregion

        #region Public Methods

        /// <summary>
        /// Navigates to the specified page
        /// </summary>
        /// <param name="page">The page to go to</param>
        /// <param name="viewModel">The view model, if any, to set explicitly to the new page</param>
        public override void GoToPage(ApplicationPage page, BaseViewModel viewModel = null)
        {
            // Change page as always
            base.GoToPage(page, viewModel);

            // Additionally, inform the view about possible sidemenu state change
            OnPropertyChanged(nameof(SideMenuVisible));
        }

        /// <summary>
        /// Navigates to the specified BeginTestPage subpage
        /// </summary>
        /// <param name="page">The page to go to</param>
        public void GoToBeginTestPage(ApplicationPage page)
        {
            // Set the current page
            CurrentBeginTestPage = page;

            // Fire off a CurrentBeginTestPage changed event
            OnPropertyChanged(nameof(CurrentBeginTestPage));
        }

        /// <summary>
        /// Closes the application
        /// </summary>
        public void Close()
        {
            Closing.Invoke();
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApplicationViewModel()
        {
            // Get the current version from assebly
            var assebly = Assembly.LoadFrom("Testinator.Server.Core.dll");
            Version = assebly.GetName().Version;
        }

        #endregion
    }
}

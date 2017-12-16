using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The application state as a view model
    /// </summary>
    public class ApplicationViewModel : BaseViewModel
    {
        /// <summary>
        /// The current page of the application
        /// </summary>
        public ApplicationPage CurrentPage { get; private set; } = ApplicationPage.Login;

        /// <summary>
        /// The view model to use for the current page when the CurrentPage changes
        /// NOTE: This is not a live up-to-date view model of the current page
        ///       it is simply used to set the view model of the current page 
        ///       at the time it changes
        /// </summary>
        public BaseViewModel CurrentPageViewModel { get; set; }

        /// <summary>
        /// The current subpage of the BeginTestPage
        /// </summary>
        public ApplicationPage CurrentBeginTestPage { get; private set; } = ApplicationPage.BeginTestInitial;

        /// <summary>
        /// Navigates to the specified page
        /// </summary>
        /// <param name="page">The page to go to</param>
        /// <param name="viewModel">The view model, if any, to set explicitly to the new page</param>
        public void GoToPage(ApplicationPage page, BaseViewModel viewModel = null)
        {
            // Set the view model
            CurrentPageViewModel = viewModel;

            // Set the current page
            CurrentPage = page;

            // Fire off a CurrentPage changed event
            OnPropertyChanged(nameof(CurrentPage));
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
    }
}

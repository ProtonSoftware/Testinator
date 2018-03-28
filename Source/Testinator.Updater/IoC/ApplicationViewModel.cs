using Testinator.Core;

namespace Testinator.Updater
{
    /// <summary>
    /// The application state as a view model
    /// </summary>
    public class ApplicationViewModel : BaseViewModel
    {
        /// <summary>
        /// Current UI page in this application
        /// </summary>
        public ApplicationPage CurrentPage { get; set; } = ApplicationPage.Initial;

        /// <summary>
        /// The view model to use for the current page when the CurrentPage changes
        /// NOTE: This is not a live up-to-date view model of the current page
        ///       it is simply used to set the view model of the current page 
        ///       at the time it changes
        /// </summary>
        public BaseViewModel CurrentPageViewModel { get; set; }

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
        }
    }
}

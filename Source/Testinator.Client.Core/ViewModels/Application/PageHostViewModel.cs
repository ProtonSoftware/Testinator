using Testinator.Core;

namespace Testinator.Client.Core
{
    /// <summary>
    /// The view model for PageHost containing informations about current page/VM of the page
    /// </summary>
    public class PageHostViewModel : BaseViewModel
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

        #region Public Helpers

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

            // Fire property changed events
            OnPropertyChanged(nameof(CurrentPageViewModel));
            OnPropertyChanged(nameof(CurrentPage));

            // Log it
            IoCClient.Logger.Log("Changing application page to:" + page.ToString());
        }

        #endregion
    }
}

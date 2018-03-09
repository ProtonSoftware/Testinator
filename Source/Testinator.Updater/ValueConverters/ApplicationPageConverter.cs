using System.Diagnostics;
using Testinator.UICore;

namespace Testinator.Updater
{
    /// <summary>
    /// Converts the <see cref="ApplicationPage"/> to an actual view/page
    /// </summary>
    public static class ApplicationPageConverter
    {
        /// <summary>
        /// Takes a <see cref="ApplicationPage"/> and a view model, if any, and creates the desired page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static BasePage ToBasePage(this ApplicationPage page, object viewModel = null)
        {
            // Find the appropriate page
            switch (page)
            {
                case ApplicationPage.Initial:
                    return new InitialPage(viewModel as InitialPageViewModel);

                case ApplicationPage.Download:
                    return new DownloadPage(viewModel as DownloadPageViewModel);

                case ApplicationPage.Exit:
                    return new ExitPage(viewModel as ExitPageViewModel);

                default:
                    Debugger.Break();
                    return null;
            }
        }

        /// <summary>
        /// Converts a <see cref="BasePage"/> to the specific <see cref="ApplicationPage"/> that is for that type of page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static ApplicationPage ToApplicationPage(this BasePage page)
        {
            // Find application page that matches the base page
            if (page is InitialPage)
                return ApplicationPage.Initial;

            if (page is DownloadPage)
                return ApplicationPage.Download;

            if (page is ExitPage)
                return ApplicationPage.Exit;

            // Alert developer of issue
            Debugger.Break();
            return default(ApplicationPage);
        }
    }
}

using System.Threading.Tasks;
using System.Windows;
using Testinator.Core;

namespace Testinator.Updater
{
    /// <summary>
    /// The view model for first initial page in this app
    /// </summary>
    public class InitialPageViewModel : BaseViewModel
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public InitialPageViewModel()
        {
            // Fade out this page after some delay
            Task.Run(async () =>
            {
                // Wait 2 sec
                await Task.Delay(2000);

                // Change the application page
                Application.Current.Dispatcher.Invoke(() => IoCUpdater.Application.GoToPage(ApplicationPage.Download));
            });
        }
    }
}

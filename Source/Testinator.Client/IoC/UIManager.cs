using System;
using System.Windows;
using Testinator.Client.Core;
using Testinator.Core;

namespace Testinator.Client
{
    /// <summary>
    /// The application implementation of the <see cref="IUIManager"/>
    /// </summary>
    public class UIManager : IUIManager
    {
        /// <summary>
        /// Changes page in the application
        /// </summary>
        /// <param name="page">Target page</param>
        /// <param name="viewmodel">Corresponding viewmodel</param>
        public void ChangePage(ApplicationPage page, BaseViewModel viewmodel = null)
        {
            // Get back on UI thread
            Application.Current.Dispatcher.Invoke(() => 
            {
                // Change page
                IoCClient.Application.GoToPage(page, viewmodel);

                // Log it
                IoCClient.Logger.Log("Changing application page to " + page.ToString());
            });
        }
    }
}

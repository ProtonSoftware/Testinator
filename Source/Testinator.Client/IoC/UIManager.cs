using System;
using System.Threading.Tasks;
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

        /// <summary>
        /// Displays a single message box to the user
        /// </summary>
        /// <param name="viewModel">The view model</param>
        /// <param name="isAlreadyOnUIThread">Indicates if caller is on UIThread, default as true</param>
        /// <returns></returns>
        public Task ShowMessage(MessageBoxDialogViewModel viewModel, bool isAlreadyOnUIThread = true)
        {
            // Prepare a dummy task to return
            var task = new Task(() => Task.Delay(1));

            // If caller isn't on UIThread, get to this thread first
            if (!isAlreadyOnUIThread)
                Application.Current.Dispatcher.BeginInvoke((Action)(() => 
                {
                    // Set the task inside UIThread
                    task = new DialogMessageBox().ShowDialog(viewModel);
                }));

            // If caller is on UIThread already, just show the dialog
            else
                task = new DialogMessageBox().ShowDialog(viewModel);

            // Finally return this task
            return task;
        }

        /// <summary>
        /// Displays a result box to the user and catch the result
        /// </summary>
        /// <param name="viewModel">The view model</param>
        /// <param name="isAlreadyOnUIThread">Indicates if caller is on UIThread, default as true</param>
        /// <returns></returns>
        public Task ShowMessage(ResultBoxDialogViewModel viewModel, bool isAlreadyOnUIThread = true)
        {
            // Prepare a dummy task to return
            var task = new Task(() => Task.Delay(1));

            // If caller isn't on UIThread already, get to this thread first
            if (!isAlreadyOnUIThread)
                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    // Set the task inside UIThread
                    task = new DialogResultBox().ShowDialog(viewModel);
                }));

            // If caller is on UIThread, just show the dialog
            else
                task = new DialogResultBox().ShowDialog(viewModel);

            // Finally return this task
            return task;
        }
    }
}

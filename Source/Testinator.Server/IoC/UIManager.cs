using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using Testinator.Core;
using Testinator.Server.Core;

namespace Testinator.Server
{
    /// <summary>
    /// The applications implementation of the <see cref="IUIManager"/>
    /// </summary>
    public class UIManager : IUIManager
    {
        /// <summary>
        /// Performs an action by taking it on dispatcher UI Thread
        /// </summary>
        /// <param name="action">An action to invoke</param>
        public void DispatcherThreadAction(Action action)
        {
            // Invoke the action on the UI Thread
            Application.Current.Dispatcher.BeginInvoke(action);
        }

        /// <summary>
        /// Displays a single message box to the user
        /// </summary>
        /// <param name="viewModel">The view model</param>
        /// <returns></returns>
        public Task ShowMessage(MessageBoxDialogViewModel viewModel)
        {
            // Prepare a dummy task to return
            Task task = null;

            // If caller isn't on UIThread, get to this thread first
            if (!Application.Current.Dispatcher.CheckAccess())
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
        /// <returns></returns>
        public Task ShowMessage(DecisionDialogViewModel viewModel)
        {
            // Prepare a dummy task to return
            Task task = null;

            // If caller isn't on UIThread already, get to this thread first
            if (!Application.Current.Dispatcher.CheckAccess())
                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    // Set the task inside UIThread
                    task = new DecisionDialogBox().ShowDialog(viewModel);
                }));

            // If caller is on UIThread, just show the dialog
            else
                task = new DecisionDialogBox().ShowDialog(viewModel);

            // Finally return this task
            return task;
        }

        /// <summary>
        /// Displays a result box to the user and catch the result
        /// </summary>
        /// <param name="viewModel">The view model</param>
        /// <returns></returns>
        public Task ShowMessage(AddLatecomersDialogViewModel viewModel)
        {
            // Prepare a dummy task to return
            Task task = null;

            // If caller isn't on UIThread already, get to this thread first
            if (!Application.Current.Dispatcher.CheckAccess())
                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    // Set the task inside UIThread
                    task = new AddLatecommersDialog().ShowDialog(viewModel);
                }));

            // If caller is on UIThread, just show the dialog
            else
                task = new AddLatecommersDialog().ShowDialog(viewModel);

            // Finally return this task
            return task;
        }
        
        /// <summary>
        /// Changes language in the application by specified language code
        /// </summary>
        /// <param name="langCode">The code of an language to change to</param>
        public void ChangeLanguage(string langCode) => LocalizationResource.Culture = new CultureInfo(langCode);

    }
}
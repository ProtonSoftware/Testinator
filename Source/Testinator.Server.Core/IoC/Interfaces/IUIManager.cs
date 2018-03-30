using System;
using System.Threading.Tasks;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The UI manager that handles any UI interaction in the application
    /// </summary>
    public interface IUIManager
    {
        /// <summary>
        /// Performs an action by taking it on dispatcher UI Thread
        /// </summary>
        /// <param name="action">An action to invoke</param>
        void DispatcherThreadAction(Action action);

        /// <summary>
        /// Displays a single message box to the user
        /// </summary>
        /// <param name="viewModel">The view model</param>
        /// <returns></returns>
        Task ShowMessage(MessageBoxDialogViewModel viewModel);

        /// <summary>
        /// Displays a result box to the user and catch the result
        /// </summary>
        /// <param name="viewModel">The view model</param>
        /// <returns></returns>
        Task ShowMessage(DecisionDialogViewModel viewModel);

        /// <summary>
        /// Displays a result box to the user and catch the result
        /// </summary>
        /// <param name="viewModel">The view model</param>
        /// <returns></returns>
        Task ShowMessage(AddLatecomersDialogViewModel viewModel);


        /// <summary>
        /// Changes language in the application by specified language code
        /// </summary>
        /// <param name="langCode">The code of an language to change to</param>
        void ChangeLanguage(string langCode);
    }
}
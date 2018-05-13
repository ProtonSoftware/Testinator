using System;
using System.Threading.Tasks;
using Testinator.Core;

namespace Testinator.Client.Core
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
        /// Enables fullscreen mode 
        /// </summary>
        void EnableFullscreenMode();

        /// <summary>
        /// Disables fullscreen mode
        /// </summary>
        void DisableFullscreenMode();

        /// <summary>
        /// Enables small aplication format
        /// </summary>
        void EnableSmallApplicationView();

        /// <summary>
        /// Disables small application format
        /// </summary>
        void DisableSmallApplicationView();

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
    }
}

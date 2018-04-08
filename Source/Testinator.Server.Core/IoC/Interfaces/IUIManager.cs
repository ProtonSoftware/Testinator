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
        /// Changes application page by making sure that we are on UI thread beforehand
        /// </summary>
        /// <param name="page">The page to change to</param>
        /// <param name="vm">The view model</param>
        void ChangeApplicationPageThreadSafe(ApplicationPage page, BaseViewModel vm = null);

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

        /// <summary>
        /// Ask the user to choose single file from the disk
        /// </summary>
        /// <param name="InitialDirectory">Initial directory</param>
        /// <param name="Filer">Filer for file types</param>
        /// <returns>Path to the file that has been selected; Empty if file has not been selected</returns>
        string ShowSingleFileDialog(string InitialDirectory, string Filer);
    }
}
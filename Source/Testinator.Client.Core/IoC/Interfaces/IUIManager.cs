using Testinator.Core;

namespace Testinator.Client.Core
{
    /// <summary>
    /// The UI manager that handles any UI interaction in the application
    /// </summary>
    public interface IUIManager
    {
        /// <summary>
        /// Changes page in the application
        /// NOTE: used as a work around thread problem in client netowrk
        /// </summary>
        /// <param name="page">The target page</param>
        /// <param name="viewmodel">The corresponding viewmodel, null by default</param>
        void ChangePage(ApplicationPage page, BaseViewModel viewmodel = null);
    }
}

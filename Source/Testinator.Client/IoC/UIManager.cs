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
            Application.Current.Dispatcher.Invoke(() => { IoCClient.Application.GoToPage(page, viewmodel); });
        }
    }
}

using System;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The view model for each item ("page-changer") in the menu list
    /// </summary>
    public class MenuListItemViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The name for this item/page
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The icon for this item
        /// </summary>
        public IconType Icon { get; set; }

        /// <summary>
        /// The page that this item leads to
        /// </summary>
        public ApplicationPage TargetPage { get; set; }

        /// <summary>
        /// Indicates if user is on this item's target page
        /// </summary>
        public bool IsSelected { get; set; } = false;

        #endregion

        #region Commands

        /// <summary>
        /// The command to set page to the this view model item's one
        /// </summary>
        public ICommand ChangePageCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MenuListItemViewModel()
        {
            // Create commands
            ChangePageCommand = new RelayCommand(ChangePage);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Changes page to specified by item one
        /// </summary>
        private void ChangePage()
        {
            // If we are on this page already, don't bother doing anything
            if (IsSelected)
                return;

            // Simply change page in the application
            IoCServer.Application.GoToPage(TargetPage);
        }

        #endregion
    }
}

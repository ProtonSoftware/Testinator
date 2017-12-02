using System.Collections.Generic;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The view model for the side menu list with "page-changers"
    /// </summary>
    public class MenuListViewModel : BaseViewModel
    {
        #region Singleton

        /// <summary>
        /// Single instance of this view model
        /// </summary>
        public static MenuListViewModel Instance => new MenuListViewModel();

        #endregion

        #region Public Properties

        /// <summary>
        /// List of items in this menu list
        /// </summary>
        public List<MenuListItemViewModel> Items { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MenuListViewModel()
        {
            Items = new List<MenuListItemViewModel>
            {
                new MenuListItemViewModel
                {
                    Name = "Home page",
                    Icon = "&#xf015;",
                    TargetPage = ApplicationPage.Home
                },
                new MenuListItemViewModel
                {
                    Name = "Login page",
                    Icon = "&#xf015;",
                    TargetPage = ApplicationPage.Login
                },
                new MenuListItemViewModel
                {
                    Name = "Home page",
                    Icon = "&#xf015;",
                    TargetPage = ApplicationPage.Home
                },
                new MenuListItemViewModel
                {
                    Name = "Home page",
                    Icon = "&#xf015;",
                    TargetPage = ApplicationPage.Home
                },
                new MenuListItemViewModel
                {
                    Name = "Login page",
                    Icon = "&#xf015;",
                    TargetPage = ApplicationPage.Login
                },
                new MenuListItemViewModel
                {
                    Name = "Home page",
                    Icon = "&#xf015;",
                    TargetPage = ApplicationPage.Home
                }
            };
        }

        #endregion
    }
}
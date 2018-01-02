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
                    Name = "Start",
                    Icon = IconType.Home,
                    TargetPage = ApplicationPage.Home
                },
                new MenuListItemViewModel
                {
                    Name = "Rozpocznij test",
                    Icon = IconType.Test,
                    TargetPage = ApplicationPage.BeginTest
                },
                new MenuListItemViewModel
                {
                    Name = "Edytor testów",
                    Icon = IconType.Editor,
                    TargetPage = ApplicationPage.TestEditor
                },
                new MenuListItemViewModel
                {
                    Name = "Wyniki testów",
                    Icon = IconType.DataBase,
                    TargetPage = ApplicationPage.TestResultsInitial,
                },
                new MenuListItemViewModel
                {
                    Name = "Screen stream",
                    Icon = IconType.Screen,
                    TargetPage = ApplicationPage.ScreenStream
                },
                new MenuListItemViewModel
                {
                    Name = "Ustawienia",
                    Icon = IconType.Settings,
                    TargetPage = ApplicationPage.Settings
                },
                new MenuListItemViewModel
                {
                    Name = "Informacje",
                    Icon = IconType.InfoCircle,
                    TargetPage = ApplicationPage.About
                },
            };
        }

        #endregion
    }
}
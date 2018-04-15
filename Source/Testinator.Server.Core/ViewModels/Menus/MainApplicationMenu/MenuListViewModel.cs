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
                    Name = LocalizationResource.Start,
                    Icon = IconType.Home,
                    TargetPage = ApplicationPage.Home
                },
                new MenuListItemViewModel
                {
                    Name = LocalizationResource.StartTest,
                    Icon = IconType.Test,
                    TargetPage = ApplicationPage.BeginTest
                },
                new MenuListItemViewModel
                {
                    Name = LocalizationResource.TestEditorTitle,
                    Icon = IconType.Editor,
                    TargetPage = ApplicationPage.TestEditorInitial
                },
                new MenuListItemViewModel
                {
                    Name = LocalizationResource.TestResults,
                    Icon = IconType.DataBase,
                    TargetPage = ApplicationPage.TestResultsInitial,
                },
                new MenuListItemViewModel
                {
                    Name = LocalizationResource.ScreenStream,
                    Icon = IconType.Screen,
                    TargetPage = ApplicationPage.ScreenStream
                },
                new MenuListItemViewModel
                {
                    Name = LocalizationResource.Settings,
                    Icon = IconType.Settings,
                    TargetPage = ApplicationPage.Settings
                },
                new MenuListItemViewModel
                {
                    Name = LocalizationResource.About,
                    Icon = IconType.InfoCircle,
                    TargetPage = ApplicationPage.About
                },
            };
        }

        #endregion
    }
}
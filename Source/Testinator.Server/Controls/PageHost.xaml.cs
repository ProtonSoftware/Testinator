using System.ComponentModel;
using System.Windows;
using Testinator.Server.Core;
using Testinator.UICore;
using System;


namespace Testinator.Server
{
    /// <summary>
    /// Interaction logic for PageHost.xaml
    /// </summary>
    public partial class PageHost : BasePageHost<ApplicationPage>
    {
        #region Singleton

        /// <summary>
        /// Single instance of this view model
        /// NOTE: It's the only way to call abstract methods like they were static ones, as we can't declare them as static
        /// </summary>
        public static PageHost Instance { get; set; } = new PageHost();

        #endregion

        #region Dependency Properties

        /// <summary>
        /// The current page to show in the page host
        /// </summary>
        public ApplicationPage CurrentPage
        {
            get => (ApplicationPage)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }

        /// <summary>
        /// Registers <see cref="CurrentPage"/> as a dependency property
        /// </summary>
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register(nameof(CurrentPage), typeof(ApplicationPage), typeof(PageHost), new UIPropertyMetadata(default(ApplicationPage), null, CurrentPagePropertyChanged));

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public PageHost() : base()
        {
            InitializeComponent();

            // If we are in DesignMode, show the current page
            // as the dependency property does not fire
            if (DesignerProperties.GetIsInDesignMode(this))
                NewPage.Content = IoCServer.Application.CurrentPage.ToBasePage();
        }

        #endregion

        #region Property Changed Events

        /// <summary>
        /// Called when the <see cref="CurrentPage"/> value has changed
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static object CurrentPagePropertyChanged(DependencyObject d, object value)
        {
            // Get target values
            var targetPage = (ApplicationPage)value;
            var targetPageViewModel = d.GetValue(CurrentPageViewModelProperty);

            // Get the frames
            var newPageFrame = (d as PageHost).NewPage;
            var oldPageFrame = (d as PageHost).OldPage;

            // Change the page based on that
            Instance.ChangeFramePages(newPageFrame, oldPageFrame, targetPage, targetPageViewModel);
            
            // Return the value back to dependency property
            return value;
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Override the base page convert to handle our application specific pages
        /// </summary>
        /// <param name="page">The page to convert as an enum</param>
        /// <param name="vm">The optional view model</param>
        public override BasePage BasePageConvert(ApplicationPage page, object vm = null) => page.ToBasePage(vm);

        /// <summary>
        /// Override the application page convert to handle our application specific pages
        /// </summary>
        /// <param name="page">The page to convert</param>
        public override ApplicationPage ApplicationPageConvert(BasePage page) => page.ToApplicationPage();

        #endregion
    }
}
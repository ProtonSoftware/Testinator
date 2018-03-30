using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Testinator.Core;

namespace Testinator.UICore
{
    /// <summary>
    /// The base for any PageHost xaml control
    /// </summary>
    public abstract class BasePageHost<T> : UserControl
        where T : IComparable
    {
        #region Dependency Properties

        /// <summary>
        /// The current page view model to show in the page host
        /// </summary>
        public BaseViewModel CurrentPageViewModel
        {
            get => (BaseViewModel)GetValue(CurrentPageViewModelProperty);
            set => SetValue(CurrentPageViewModelProperty, value);
        }

        /// <summary>
        /// Registers <see cref="CurrentPageViewModel"/> as a dependency property
        /// </summary>
        public static readonly DependencyProperty CurrentPageViewModelProperty =
            DependencyProperty.Register(nameof(CurrentPageViewModel),
                typeof(BaseViewModel), typeof(BasePageHost<T>),
                new UIPropertyMetadata());

        #endregion

        #region Protected Helpers

        protected void ChangeFramePages(ContentControl newPageFrame, ContentControl oldPageFrame, T targetPage, object targetPageViewModel)
        {
            // If the current page hasn't changed
            // just update the view model
            if (newPageFrame.Content is BasePage page &&
                ApplicationPageConvert(page).CompareTo(targetPage) == 0)
            {
                // Just update the view model (if it isn't null)
                if (targetPageViewModel != null)
                    page.ViewModelObject = targetPageViewModel;

                return;
            }

            // Store the current page content as the old page
            var oldPageContent = newPageFrame.Content;

            // Remove current page from new page frame
            newPageFrame.Content = null;

            // Move the previous page into the old page frame
            oldPageFrame.Content = oldPageContent;

            // Animate out previous page when the Loaded event fires
            // right after this call due to moving frames
            if (oldPageContent is BasePage oldPage)
            {
                // Tell old page to animate out
                oldPage.ShouldAnimateOut = true;

                // Once it is done, remove it
                Task.Delay((int)(oldPage.SlideSeconds * 1000)).ContinueWith((t) =>
                {
                    // Remove old page
                    Application.Current.Dispatcher.Invoke(() => oldPageFrame.Content = null);
                });
            }

            // Set the new page content
            newPageFrame.Content = BasePageConvert(targetPage, targetPageViewModel);
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Returns Application page from <see cref="BasePage"/>
        /// </summary>
        public abstract T ApplicationPageConvert(BasePage page);

        /// <summary>
        /// Returns <see cref="BasePage"/> from Application page
        /// </summary>
        public abstract BasePage BasePageConvert(T page, object vm = null);

        #endregion
    }
}
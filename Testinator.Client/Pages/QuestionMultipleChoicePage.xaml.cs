using Testinator.Client.Core;
using Testinator.UICore;
using System.Windows;

namespace Testinator.Client
{
    /// <summary>
    /// Interaction logic for QuestionMultipleChoicePage.xaml
    /// </summary>
    public partial class QuestionMultipleChoicePage : BasePage<LoginViewModel>
    {
        #region Constructor

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public QuestionMultipleChoicePage(LoginViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();

            // Set the default unload animation
            PageUnloadAnimation = PageAnimation.SlideAndFadeOutToLeft;
        }

        #endregion
    }
}

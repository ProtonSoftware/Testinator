using Testinator.AnimationFramework;
using Testinator.Client.Core;
using Testinator.UICore;

namespace Testinator.Client
{
    /// <summary>
    /// Interaction logic for QuestionSingleTextBoxPage.xaml
    /// </summary>
    public partial class QuestionSingleTextBoxPage : BasePage<QuestionSingleTextBoxViewModel>
    {
        #region Constructor

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public QuestionSingleTextBoxPage(QuestionSingleTextBoxViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();

            // Set default animations
            PageLoadAnimation = ElementAnimation.SlideAndFadeInFromRight;
            PageUnloadAnimation = ElementAnimation.SlideAndFadeOutToLeft;
        }

        #endregion
    }
}

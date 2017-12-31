using Testinator.Client.Core;
using Testinator.UICore;

namespace Testinator.Client
{
    /// <summary>
    /// Interaction logic for QuestionMultipleChoicePage.xaml
    /// </summary>
    public partial class QuestionMultipleChoicePage : BasePage<QuestionMultipleChoiceViewModel>
    {
        #region Constructor

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public QuestionMultipleChoicePage(QuestionMultipleChoiceViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();

            // Set default animations
            PageLoadAnimation = PageAnimation.SlideAndFadeInFromRight;
            PageUnloadAnimation = PageAnimation.SlideAndFadeOutToLeft;
        }

        #endregion
    }
}

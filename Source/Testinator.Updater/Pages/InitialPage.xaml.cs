using Testinator.AnimationFramework;
using Testinator.UICore;

namespace Testinator.Updater
{
    /// <summary>
    /// Interaction logic for InitialPage.xaml
    /// </summary>
    public partial class InitialPage : BasePage<InitialPageViewModel>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public InitialPage() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public InitialPage(InitialPageViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();

            // Change default animation
            PageUnloadAnimation = ElementAnimation.SlideAndFadeOutToRight;
        }

        #endregion
    }
}

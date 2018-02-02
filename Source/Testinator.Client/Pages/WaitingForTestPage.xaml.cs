using Testinator.AnimationFramework;
using Testinator.Client.Core;
using Testinator.UICore;

namespace Testinator.Client
{
    /// <summary>
    /// Interaction logic for WaitingForTestPage.xaml
    /// </summary>
    public partial class WaitingForTestPage : BasePage<WaitingForTestViewModel>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public WaitingForTestPage() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public WaitingForTestPage(WaitingForTestViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();

            // Set the default page load/unload animation
            PageLoadAnimation = ElementAnimation.SlideAndFadeInFromRight;
            PageUnloadAnimation = ElementAnimation.SlideAndFadeOutToLeft;
        }

        #endregion
    }
}

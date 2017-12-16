using Testinator.Client.Core;
using Testinator.UICore;
using System.Windows;

namespace Testinator.Client
{
    /// <summary>
    /// Interaction logic for ResultPage.xaml
    /// </summary>
    public partial class ResultPage : BasePage<ResultViewModel>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ResultPage() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public ResultPage(ResultViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();

            // Set the default page load/unload animation
            PageLoadAnimation = PageAnimation.SlideAndFadeInFromRight;
            PageUnloadAnimation = PageAnimation.SlideAndFadeOutToLeft;
        }

        #endregion
    }
}

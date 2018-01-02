using Testinator.Server.Core;
using Testinator.UICore;

namespace Testinator.Server
{
    /// <summary>
    /// Interaction logic for AboutPage.xaml
    /// </summary>
    public partial class TestResultsInitialPage : BasePage<TestResultsViewModel>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestResultsInitialPage() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public TestResultsInitialPage(TestResultsViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();
        }
        #endregion
    }
}

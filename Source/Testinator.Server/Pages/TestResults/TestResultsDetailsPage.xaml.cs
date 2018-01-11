using Testinator.Server.Core;
using Testinator.UICore;

namespace Testinator.Server
{
    /// <summary>
    /// Interaction logic for TestResultsDetailsPage.xaml
    /// </summary>
    public partial class TestResultsDetailsPage : BasePage<TestResultsDetailsViewModel>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestResultsDetailsPage() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public TestResultsDetailsPage(TestResultsDetailsViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();
        }
        #endregion
    }
}

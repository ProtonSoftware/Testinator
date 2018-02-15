using System.Windows.Controls;
using Testinator.Server.Core;
using Testinator.UICore;

namespace Testinator.Server
{
    /// <summary>
    /// Interaction logic for TestResultsDetailsViewPage.xaml
    /// </summary>
    public partial class TestResultsDetailsViewPage : BasePage<TestResultsDetailsViewModel>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public TestResultsDetailsViewPage() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public TestResultsDetailsViewPage(TestResultsDetailsViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();
        }
    }
}

using System.Windows.Controls;
using Testinator.Server.Core;
using Testinator.UICore;

namespace Testinator.Server
{
    /// <summary>
    /// Interaction logic for TestResultsStudentsViewPage.xaml
    /// </summary>
    public partial class TestResultsStudentsViewPage : BasePage<TestResultsDetailsViewModel>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public TestResultsStudentsViewPage() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public TestResultsStudentsViewPage(TestResultsDetailsViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();
        }
    }
}

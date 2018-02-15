using System.Windows.Controls;
using Testinator.Server.Core;
using Testinator.UICore;

namespace Testinator.Server
{
    /// <summary>
    /// Interaction logic for TestResultsQuestionsViewPage.xaml
    /// </summary>
    public partial class TestResultsQuestionsViewPage : BasePage<TestResultsDetailsViewModel>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public TestResultsQuestionsViewPage() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public TestResultsQuestionsViewPage(TestResultsDetailsViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();
        }
    }
}

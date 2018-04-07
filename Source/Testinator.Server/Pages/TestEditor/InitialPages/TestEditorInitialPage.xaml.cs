using Testinator.Server.Core;
using Testinator.UICore;

namespace Testinator.Server
{
    /// <summary>
    /// Interaction logic for TestEditorPage.xaml
    /// </summary>
    public partial class TestEditorInitialPage : BasePage<TestEditorInitialPageViewModel>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestEditorInitialPage() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public TestEditorInitialPage(TestEditorInitialPageViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();
        }

        #endregion
    }
}

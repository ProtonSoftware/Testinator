using Testinator.Server.Core;
using Testinator.UICore;

namespace Testinator.Server
{
    /// <summary>
    /// Interaction logic for TestEditorAttachCriteriaPage.xaml
    /// </summary>
    public partial class TestEditorAttachCriteriaPage : BasePage<TestEditorAttachCriteriaViewModel>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestEditorAttachCriteriaPage() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public TestEditorAttachCriteriaPage(TestEditorAttachCriteriaViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();
        }

        #endregion
    }
}

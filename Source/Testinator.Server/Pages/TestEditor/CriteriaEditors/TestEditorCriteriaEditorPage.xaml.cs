using Testinator.Server.Core;
using Testinator.UICore;

namespace Testinator.Server
{
    /// <summary>
    /// Interaction logic for TestEditorAddNewCriteriaPage.xaml
    /// </summary>
    public partial class TestEditorCriteriaEditorPage : BasePage<TestEditorCriteriaEditorViewModel>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestEditorCriteriaEditorPage() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public TestEditorCriteriaEditorPage(TestEditorCriteriaEditorViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();
        }

        #endregion
    }
}

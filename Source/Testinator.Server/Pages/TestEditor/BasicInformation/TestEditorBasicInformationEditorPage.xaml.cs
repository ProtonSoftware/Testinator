using Testinator.Server.Core;
using Testinator.UICore;

namespace Testinator.Server
{
    /// <summary>
    /// Interaction logic for TestEditorAddNewTestPage.xaml
    /// </summary>
    public partial class TestEditorBasicInformationEditorPage : BasePage<TestEditorBasicInformationEditorViewModel>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestEditorBasicInformationEditorPage() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public TestEditorBasicInformationEditorPage(TestEditorBasicInformationEditorViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();
        }

        #endregion
    }
}

using Testinator.Server.Core;
using Testinator.UICore;

namespace Testinator.Server
{
    /// <summary>
    /// Interaction logic for TestEditorAddQuestionsPage.xaml
    /// </summary>
    public partial class TestEditorQuestionsEditorPage : BasePage<TestEditorQuestionsEditorViewModel>
    {
        #region Constructor

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public TestEditorQuestionsEditorPage(TestEditorQuestionsEditorViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();
        }

        #endregion
    }
}

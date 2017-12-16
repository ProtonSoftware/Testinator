using Testinator.Server.Core;
using Testinator.UICore;

namespace Testinator.Server
{
    /// <summary>
    /// Interaction logic for BeginTestInitialPage.xaml
    /// </summary>
    public partial class BeginTestInitialPage : BasePage<BeginTestViewModel>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BeginTestInitialPage() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public BeginTestInitialPage(BeginTestViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();
        }

        #endregion
    }
}

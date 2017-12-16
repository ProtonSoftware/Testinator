using Testinator.Server.Core;
using Testinator.UICore;

namespace Testinator.Server
{
    /// <summary>
    /// Interaction logic for BeginTestInfoPage.xaml
    /// </summary>
    public partial class BeginTestInfoPage : BasePage<BeginTestViewModel>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BeginTestInfoPage() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public BeginTestInfoPage(BeginTestViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();
        }

        #endregion
    }
}

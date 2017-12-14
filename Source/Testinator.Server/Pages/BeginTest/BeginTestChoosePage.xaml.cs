using Testinator.Server.Core;
using Testinator.UICore;

namespace Testinator.Server
{
    /// <summary>
    /// Interaction logic for BeginTestChoosePage.xaml
    /// </summary>
    public partial class BeginTestChoosePage : BasePage<BeginTestViewModel>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BeginTestChoosePage() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public BeginTestChoosePage(BeginTestViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();
        }

        #endregion
    }
}

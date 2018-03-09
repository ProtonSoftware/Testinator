using Testinator.UICore;

namespace Testinator.Updater
{
    /// <summary>
    /// Interaction logic for ExitPage.xaml
    /// </summary>
    public partial class ExitPage : BasePage<ExitPageViewModel>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ExitPage() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public ExitPage(ExitPageViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();
        }

        #endregion
    }
}

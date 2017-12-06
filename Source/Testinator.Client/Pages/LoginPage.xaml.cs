using Testinator.Client.Core;
using Testinator.UICore;
using System.Windows;

namespace Testinator.Client
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : BasePage<LoginViewModel>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginPage() : base()
        {
            InitializeComponent();

            // Disable animation on this page
            PageLoadAnimation = PageAnimation.None;
            Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public LoginPage(LoginViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();

            // Disable animation on this page
            PageLoadAnimation = PageAnimation.None;
            Visibility = Visibility.Visible;
        }

        #endregion
    }
}

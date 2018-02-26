using System.Reflection;
using Testinator.Core;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The view model for the about page
    /// </summary>
    public class AboutViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The current version of this application as a string
        /// </summary>
        public string ApplicationVersion { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public AboutViewModel()
        {
            // Get the current version of this app
            ApplicationVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        #endregion
    }
}

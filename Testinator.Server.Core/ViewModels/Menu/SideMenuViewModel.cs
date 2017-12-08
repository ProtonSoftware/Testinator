using Testinator.Core;
using System.Windows.Input;

namespace Testinator.Server.Core
{
    /// <summary>
    /// The view model for the side menu
    /// </summary>
    public class SideMenuViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Indicates if side menu should be expanded
        /// </summary>
        public bool Expanded { get; set; } = true;

        #endregion

        #region Commands

        /// <summary>
        /// The command to expand/collapse the side menu
        /// </summary>
        public ICommand MenuExpandCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SideMenuViewModel()
        {
            // Create commands
            MenuExpandCommand = new RelayCommand(ExpandMenu);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Expands/collapses the menu
        /// </summary>
        private void ExpandMenu()
        {
            // Toogle the expanded menu flag
            Expanded ^= true;
        }

        #endregion
    }
}

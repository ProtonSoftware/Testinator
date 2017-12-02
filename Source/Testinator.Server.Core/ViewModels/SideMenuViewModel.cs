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
        /// Indicates if the side menu is expanded (default as true)
        /// </summary>
        public bool IsMenuExpanded { get; set; } = true;

        /// <summary>
        /// Current width of the side menu which depends on whether menu is expanded or not
        /// </summary>
        public int CurrentWidth => IsMenuExpanded ? 200 : 50;

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
            IsMenuExpanded ^= true;
        }

        #endregion
    }
}

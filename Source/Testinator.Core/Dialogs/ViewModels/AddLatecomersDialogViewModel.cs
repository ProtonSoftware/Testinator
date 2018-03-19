using System.Collections.Generic;

namespace Testinator.Core
{
    /// <summary>
    /// A viewmodel for dialog box to show possible users that can be added to the current test sessions,
    /// and ask the user to determine who they want to add to the test
    /// </summary>
    public class AddLatecomersDialogViewModel: BaseDialogViewModel
    {
        /// <summary>
        /// The message to display
        /// </summary>
        public string Message { get; set; }

        public string Test { get; set; }
        /// <summary>
        /// Clients that can be added to the test
        /// </summary>
        public List<ClientModel> ClientsToChoose { get; set; }

        /// <summary>
        /// The clients user agreed to add
        /// </summary>
        public List<ClientModel> UserResponse { get; set; } = new List<ClientModel>();
    }
}

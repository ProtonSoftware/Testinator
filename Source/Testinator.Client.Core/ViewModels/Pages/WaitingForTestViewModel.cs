using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Testinator.Core;

namespace Testinator.Client.Core
{
    /// <summary>
    /// The view model for "waiting for test" page
    /// </summary>
    public class WaitingForTestViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The current users name
        /// </summary>
        public TextEntryViewModel Name { get; set; }

        /// <summary>
        /// The current users surname
        /// </summary>
        public TextEntryViewModel Surname { get; set; }

        /// <summary>
        /// The specified user name
        /// </summary>
        /*public string Name
        {
            get => IoCClient.Client.ClientName;
            set
            {
                IoCClient.Client.ClientName = value;
            }
        }

        /// <summary>
        /// The specified user surname
        /// </summary>
        public string Surname
        {
            get => IoCClient.Client.ClientSurname;
            set
            {
                IoCClient.Client.ClientSurname = value;
            }
        }*/

        /// <summary>
        /// A flag indicating if we have any test to show
        /// </summary>
        public bool ReceivedTest { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public WaitingForTestViewModel()
        {
            // Set input data
            Name = new TextEntryViewModel { Label = "Imię", OriginalText = IoCClient.Client.ClientName };
            Surname = new TextEntryViewModel { Label = "Nazwisko", OriginalText = IoCClient.Client.ClientSurname };
        }

        #endregion
    }
}

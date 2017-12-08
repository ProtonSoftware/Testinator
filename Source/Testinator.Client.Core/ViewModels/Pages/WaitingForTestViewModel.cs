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
        /// The specified user name
        /// </summary>
        public string Name
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
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public WaitingForTestViewModel()
        {

        }

        #endregion
    }
}

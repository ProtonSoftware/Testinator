using System;
using System.Windows.Input;
using Testinator.Core;

namespace ClientTesting
{
    public class ViewModelClient : BaseViewModel
    {
        public string Ip { get; set; } = "127.0.0.1";

        public int Port { get; set; } = 3333;

        public ICommand StartCommand { get; set; }

        public ICommand StopCommand { get; set; }

        public bool StartPossible { get; set; } = true;

        public bool StopPossible { get; set; } = false;

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ViewModelClient()
        {
            StartCommand = new RelayCommand(Start);
            StopCommand = new RelayCommand(Stop);
                
        }

        private void Stop()
        {
            throw new NotImplementedException();
        }

        private void Start()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

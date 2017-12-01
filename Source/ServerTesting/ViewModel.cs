using System;
using System.Text;
using System.Windows.Input;
using Testinator.Core;
using Testinator.Network.Server;

namespace ServerTesting
{
    public class ViewModel : BaseViewModel
    {

        public int Test { get; set; } = 1;

        public string Ip { get; set; } = "127.0.0.1";

        public int Port { get; set; } = 3333;

        public ICommand StartCommand { get; set; }

        public ICommand StopCommand { get; set; }

        public bool StartPossible { get; set; } = true;

        public bool StopPossible { get; set; } = false;

        public string Message { get; set; }

        public ServerBase Server { get; set; }


        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ViewModel()
        {
            StartCommand = new RelayCommand(Start);
            StopCommand = new RelayCommand(Stop);
        }

        private void Stop()
        {
            StartPossible = true;
            StopPossible = false;

            Server.Stop();
        }

        private void Receive(byte[] data)
        {
            string msg = Encoding.ASCII.GetString(data);
            
        }

        private void Start()
       {
            Server = new ServerBase(Ip, Port)
            {
                ReceiverCallback = Receive
            };
            Server.Start();

            StartPossible = false;
            StopPossible = true;
        }

        #endregion
    }
}

using System;
using System.Text;
using System.Windows.Input;
using Testinator.Core;
using Testinator.Network.Server;

namespace ServerTesting
{
    public class ViewModel : BaseViewModel
    { 

        public ServerBase Server { get; set; } = new ServerBase();

        public string Ip { get; set; } = "127.0.0.1";

        public int Port { get; set; } = 3333;

        public ICommand StartCommand { get; set; }

        public ICommand StopCommand { get; set; }

        public bool StartPossible { get; set; } = true;

        public bool StopPossible { get; set; } = false;

        public string Message { get; set; }

        public int ClientNumber => Server.ConnectedClientCount;

        public ICommand ClearCommand { get; set; }

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ViewModel()
        {
            StartCommand = new RelayCommand(Start);
            StopCommand = new RelayCommand(Stop);
            ClearCommand = new RelayCommand(Clear);
        }

        private void Clear()
        {
            Message = string.Empty;
        }

        #endregion

        private void Start()
        {
            Server.SetIP(Ip);
            Server.Port = Port;
            Server.ReceiverCallback = Receive;
            Server.ClientConnectedCallback = ClientConnected;
            Server.ClientDisconnectedCallback = ClientDisconnected;
            Server.Start();

            StartPossible = false;
            StopPossible = true;
        }

        private void ClientDisconnected()
        {
            OnPropertyChanged(nameof(ClientNumber));
        }

        private void ClientConnected()
        {
            OnPropertyChanged(nameof(ClientNumber));
        }

        private void Stop()
        {

            Server.Stop();
            StartPossible = true;
            StopPossible = false;
        }

        private void Receive(byte[] data)
        {
            string msg = Encoding.ASCII.GetString(data);
            Message += msg + "\n";            
        }
    }
}

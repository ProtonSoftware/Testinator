using System;
using System.Windows.Input;
using Testinator.Core;
using Testinator.Core.Network;
using Testinator.Network.Client;
using Testinator.Network.Server;

namespace ClientTesting
{
    public class ViewModelClient : BaseViewModel
    {
        public string Ip { get; set; } = "127.0.0.1";

        public int Port { get; set; } = 3333;

        public ICommand StartCommand { get; set; }

        public ICommand StopCommand { get; set; }

        public ICommand SendCommand { get; set; }

        public bool StartPossible { get; set; } = true;

        public bool StopPossible { get; set; } = false;

        public string Name { get; set; }

        public string Surname{ get; set; }

        private ClientNetwork client = new ClientNetwork();

        public string Connected { get; set; } = "Disconnected!";
        
        public int Attempts => client.Attempts;

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ViewModelClient()
        {
            StartCommand = new RelayCommand(Start);
            StopCommand = new RelayCommand(Stop);
            SendCommand = new RelayCommand(Send);
            Ip = client.IPAddress.ToString();
            Port = client.Port;
            Name = "Jack";
            Surname = "Sparrow";
            client.ConnectedCallback = ConnectedCallback;
            client.DataRecivedCallback = DataRecivedCallabck;
            client.DisconnectedCallback = DisconnectCallback;
                            
        }

        private void Send()
        {
            if (client.IsConnected)
            {
                client.SendData(new DataPackage(PackageType.Info, new InfoPackage(Environment.MachineName, MacAddressHelpers.GetMac(), Name, Surname)));
            }
        }

        private void DisconnectCallback()
        {
            Connected = "Disconnected";
        }

        private void DataRecivedCallabck(DataPackage data)
        {
        }

        private void ConnectedCallback()
        {
            Connected = "Connected";
        }

        private void Stop()
        {
            StartPossible = true;
            StopPossible = false;
            client.Disconnect();
        }

        private void Start()
        {
            StartPossible = false;
            StopPossible = true;
            client.StartConnecting();
        }

        #endregion
    }
}

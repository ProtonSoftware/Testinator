using System;
using System.Net;
using System.Windows.Input;
using Testinator.Core;
using Testinator.Core.Network;
using Testinator.Network.Client;

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

        public string Surname { get; set; }

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
            client.SendData(new DataPackage(PackageType.Info, new InfoPackage(Environment.MachineName, MacAddressHelpers.GetMac(), Name, Surname)));

        }

        private void DisconnectCallback()
        {
            StartPossible = true;
            StopPossible = false;
            Connected = "Disconnected";
        }

        private void DataRecivedCallabck(DataPackage data)
        {
        }

        private void ConnectedCallback()
        {
            Connected = "Connected";
            client.SendData(new DataPackage(PackageType.Info, new InfoPackage(Environment.MachineName, MacAddressHelpers.GetMac(), Name, Surname)));
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
            client.IPAddress = IPAddress.Parse(Ip);
            client.Port = Port;
            client.StartConnecting();
        }

        #endregion
    }
}
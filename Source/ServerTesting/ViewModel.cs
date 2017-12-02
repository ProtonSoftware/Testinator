using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System.Windows.Threading;
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


        private ObservableCollection<ClientModel> _Clients = new ObservableCollection<ClientModel>();
        public ObservableCollection<ClientModel> Clients
        {
            get { return _Clients; }
            set { _Clients = value; }
        }

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
            Server.DataRecivedCallback = Receive;
            Server.ClientConnectedCallback = ClientConnected;
            Server.ClientDisconnectedCallback = ClientDisconnected;
            Server.Start();

            StartPossible = false;
            StopPossible = true;
        }

        private void ClientDisconnected(ClientModel sender)
        {
            OnPropertyChanged(nameof(ClientNumber));
            App.Current.Dispatcher.Invoke(() => { Clients.Remove(sender); });
        }

        private void ClientConnected(ClientModel sender)
        {
            OnPropertyChanged(nameof(ClientNumber));
            App.Current.Dispatcher.Invoke(() => { Clients.Add(sender); });

        }

        private void Stop()
        {

            Server.Stop();
            StartPossible = true;
            StopPossible = false;
        }

        private void Receive(ClientModel sender, DataPackage data)
        {
            
        }
    }
}

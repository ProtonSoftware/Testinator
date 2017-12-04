using System.Collections.ObjectModel;
using System.Windows.Input;
using Testinator.Core;
using Testinator.Network.Server;

namespace ServerTestingIMPLEMENTTHISINNORMALSERVERUINOW
{
    public class ViewModelServer : BaseViewModel
    {
        public Server Server { get; set; } = new Server();

        public string Ip { get; set; }

        public int Port { get; set; }

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
        public ViewModelServer()
        {
            StartCommand = new RelayCommand(Start);
            StopCommand = new RelayCommand(Stop);
            ClearCommand = new RelayCommand(Clear);
            Ip = Server.Ip;
            Port = Server.Port;
        }

        private void Clear()
        {
            Message = string.Empty;
        }

        #endregion

        private void Start()
        {
            Server.Port = Port;
            Server.Ip = Ip;
            Server.OnDataRecived += Receive;
            Server.OnClientConnected += ClientConnected;
            Server.OnClientDisconnected += ClientDisconnected;
            Server.OnClientDataUpdated += ClientUpdated;
            Server.Start();

            StartPossible = false;
            StopPossible = true;
        }

        private void ClientUpdated(ClientModel old, ClientModel New)
        {
            int idx = Clients.IndexOf(old);
            if (idx != -1)
            {
                App.Current.Dispatcher.Invoke(() => { Clients[idx] = New; });
            }
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
            Clients.Clear();
            StartPossible = true;
            StopPossible = false;
        }

        private void Receive(ClientModel sender, DataPackage data)
        {
            //App.Current.Dispatcher.Invoke(() => { Clients[Clients.IndexOf(sender)].MachineName = sender.MachineName; });
            //App.Current.Dispatcher.Invoke(() => { Clients.Add(sender); });
        }
    }
}

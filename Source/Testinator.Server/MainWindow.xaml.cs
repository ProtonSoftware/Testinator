using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Testinator.Network.Server;

namespace Testinator.Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public ServerBase Server { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Server = new ServerBase(txtIp.Text, Int32.Parse(txtPort.Text))
            {
                ReceiverCallback = Receive
            };
            Server.Start();

            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;
        }

        private void Receive(byte[] data)
        {
            string msg = Encoding.ASCII.GetString(data);
            Dispatcher.Invoke(() => { txtRec.Text += msg + "\n"; });
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;

            Server.Stop();
        }
    }
}

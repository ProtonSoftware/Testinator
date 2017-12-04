using System.Windows;

namespace ServerTestingIMPLEMENTTHISINNORMALSERVERUINOW
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.DataContext = new ViewModelServer();
            InitializeComponent();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            (DataContext as ViewModelServer).StopCommand.Execute(null);
        }
    }
}

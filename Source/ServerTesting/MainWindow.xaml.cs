using System.Windows;


namespace ServerTesting
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

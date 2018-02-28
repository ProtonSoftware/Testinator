using System.Windows;

namespace Testinator.Updater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Set data context for this window to allow data binding
            DataContext = new WindowViewModel(this);
        }
    }
}

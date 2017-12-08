using System.Windows;

namespace Testinator.Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Set DataContext to the WindowViewModel to allow binding in xaml
            DataContext = new WindowViewModel(this);
        }
    }
}

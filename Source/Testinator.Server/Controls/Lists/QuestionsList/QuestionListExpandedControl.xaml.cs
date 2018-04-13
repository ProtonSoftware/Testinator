using System.Windows.Controls;
using Testinator.Server.Core;

namespace Testinator.Server
{
    /// <summary>
    /// Interaction logic for QuestionListExpandedControl.xaml
    /// </summary>
    public partial class QuestionListExpandedControl : UserControl
    {
        public QuestionListExpandedControl()
        {
            InitializeComponent();
            DataContext = QuestionListViewModel.Instance;
        }
    }
}

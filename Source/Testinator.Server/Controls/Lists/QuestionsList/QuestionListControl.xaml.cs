using System.Windows.Controls;
using Testinator.Server.Core;

namespace Testinator.Server
{
    /// <summary>
    /// Interaction logic for QuestionListControl.xaml
    /// </summary>
    public partial class QuestionListControl : UserControl
    {
        public QuestionListControl()
        {
            InitializeComponent();

            // Set data context		
            DataContext = QuestionListViewModel.Instance;
        }
    }
}

using System.Windows.Controls;
using Testinator.Server.Core;

namespace Testinator.Server
{
    /// <summary>
    /// Interaction logic for CriteriaListControl.xaml
    /// </summary>
    public partial class CriteriaListControl : UserControl
    {
        public CriteriaListControl()
        {
            InitializeComponent();
            DataContext = CriteriaListViewModel.Instance;
        }
    }
}

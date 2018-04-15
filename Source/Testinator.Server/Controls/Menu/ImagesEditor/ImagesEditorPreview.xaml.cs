using System.Windows.Controls;
using Testinator.Server.Core;

namespace Testinator.Server
{
    /// <summary>
    /// Interaction logic for ImagesEditorPreview.xaml
    /// </summary>
    public partial class ImagesEditorPreview : UserControl
    {
        public ImagesEditorPreview()
        {
            InitializeComponent();
            DataContext = ImagesEditorViewModel.Instance;
        }
    }
}

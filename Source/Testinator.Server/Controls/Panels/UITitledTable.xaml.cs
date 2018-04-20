using System.Windows;
using System.Windows.Controls;

namespace Testinator.Server
{
    /// <summary>
    /// Interaction logic for UITitledTable.xaml
    /// </summary>
    public partial class UITitledTable : UserControl
    {
        #region Dependency Properties

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(UITitledTable), new PropertyMetadata(string.Empty));

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public UITitledTable()
        {
            InitializeComponent();
        }

        #endregion
    }
}

using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Testinator.Server
{
    /// <summary>
    /// Interaction logic for TextWithIcon.xaml
    /// </summary>
    public partial class TextWithIcon : UserControl
    {
        #region Dependency Properties

        /// <summary>
        /// Font awesome's icon we want to show
        /// </summary>
        public string Icon
        {
            get => (string)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(string), typeof(TextWithIcon), new PropertyMetadata(default(string)));

        /// <summary>
        /// Color of both text and icon
        /// </summary>
        public SolidColorBrush ForegroundColor
        {
            get => (SolidColorBrush)GetValue(ForegroundColorProperty);
            set => SetValue(ForegroundColorProperty, value);
        }

        // Using a DependencyProperty as the backing store for ForegroundColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ForegroundColorProperty =
            DependencyProperty.Register(nameof(ForegroundColor), typeof(SolidColorBrush), typeof(TextWithIcon), new PropertyMetadata(default(SolidColorBrush)));

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TextWithIcon()
        {
            InitializeComponent();
        }

        #endregion
    }
}

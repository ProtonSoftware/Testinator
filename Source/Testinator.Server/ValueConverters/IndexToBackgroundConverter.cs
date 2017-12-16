using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using Testinator.UICore;

namespace Testinator.Server
{
    /// <summary>
    /// A converter that takes in a index and returns a <see cref="Background"/> color
    /// </summary>
    public class IndexToBackgroundConverter : BaseValueConverter<IndexToBackgroundConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Cast value and parameter to integer
            int index = Int32.Parse(value.ToString());
            int param = Int32.Parse(parameter.ToString());

            // Check if they match
            if (index == param)
                // Return green color
                return Application.Current.FindResource("GreenLightBrush");
            else
                // Return transparent color
                return Color.FromArgb(255, 255, 255, 255);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
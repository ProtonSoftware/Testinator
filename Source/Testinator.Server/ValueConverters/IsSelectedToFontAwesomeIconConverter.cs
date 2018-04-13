using System;
using System.Globalization;
using Testinator.UICore;

namespace Testinator.Server
{
    /// <summary>
    /// A converter that takes in a boolean (IsCollapsed state variable) and return fontawesome icon based on it
    /// </summary>
    public class IsSelectedToFontAwesomeIconConverter : BaseValueConverter<IsSelectedToFontAwesomeIconConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Value - IsCollapsed
            if ((bool)value)
                return "\uf101";

            return "\uf100";
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
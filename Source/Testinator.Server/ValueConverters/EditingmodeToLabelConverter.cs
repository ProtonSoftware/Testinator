using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using Testinator.UICore;

namespace Testinator.Server
{
    /// <summary>
    /// Converts boolean value to the corresponding string name
    /// </summary>
    public class EditingmodeToLabelConverter : BaseValueConverter<EditingmodeToLabelConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return "Zapisz";
            else
                return "Dodaj";
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using Testinator.UICore;

namespace Testinator.Server
{
    /// <summary>
    /// A converter that takes in a number and another number as a parameter, if numbers are equal return visible, otherwise hidden
    /// </summary>
    public class AnswerIndexToVisibiltyConverter : BaseValueConverter<AnswerIndexToVisibiltyConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var valueidx = (int)value;
                var number = int.Parse((string)parameter);
                return valueidx == number ? Visibility.Visible : Visibility.Hidden;
            }
            catch
            {
                return Visibility.Hidden;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
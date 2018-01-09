using System;
using System.Globalization;
using System.Windows;
using Testinator.Core;
using Testinator.UICore;

namespace Testinator.Server
{
    /// <summary>
    /// A converter that takes in a <see cref="Marks"/>
    /// </summary>
    public class MarkToTextConverter : BaseValueConverter<MarkToTextConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Cast value to Mark enumerator
            var mark = (Marks)value;

            // Based on mark...
            switch(mark)
            {
                case Marks.A:
                    return LocalizationResource.MarkAName;
                case Marks.B:
                    return LocalizationResource.MarkBName;
                case Marks.C:
                    return LocalizationResource.MarkCName;
                case Marks.D:
                    return LocalizationResource.MarkDName;
                case Marks.E:
                    return LocalizationResource.MarkEName;
                case Marks.F:
                    return LocalizationResource.MarkFName;
                default:
                    return string.Empty;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
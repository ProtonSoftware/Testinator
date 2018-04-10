using System;
using System.Globalization;
using System.Windows;
using Testinator.Core;
using Testinator.UICore;

namespace Testinator.Server
{
    /// <summary>
    /// A converter that takes in a <see cref="QuestionType"/> and returns visibility based on it
    /// </summary>
    public class QuestionTypeToVisibilityConverter : BaseValueConverter<QuestionTypeToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var CurrentQuestion = (QuestionType)value;
                var ThisQuestion = (QuestionType)parameter;

                // If question type is the same as parameter, make it visible
                if (CurrentQuestion == ThisQuestion)
                    return Visibility.Visible;

                // Otherwise, collapse it
                else
                    return Visibility.Collapsed;
            }
            catch
            {
                return Visibility.Collapsed;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
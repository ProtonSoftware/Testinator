using System;
using System.Globalization;
using Testinator.UICore;
using Testinator.Core;
using Testinator.Client.Core;

namespace Testinator.Client
{
    /// <summary>
    /// A converter that takes in a <see cref="QuestionType"/> and returns 
    /// the FontAwesome string based on that
    /// </summary>
    public class QuestionTypeToIconConverter : BaseValueConverter<QuestionTypeToIconConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Based on type...
            var type = (QuestionType)value;
            switch(type)
            {
                case QuestionType.MultipleChoice:
                    return (IconType.MultipleChoiceQuestion).ToFontAwesome();

                case QuestionType.MultipleCheckboxes:
                    return (IconType.MultipleCheckBoxesQuestion).ToFontAwesome();

                case QuestionType.SingleTextBox:
                    return (IconType.SingleTextBoxQuestion).ToFontAwesome();

                default:
                    // Icon not found, something went wrong
                    return default(IconType);
            }
            
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

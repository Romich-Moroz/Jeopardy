using Jeopardy.Core.Data.Quiz;
using Jeopardy.Core.Data.Quiz.Constants;
using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Jeopardy.Core.Wpf.Converters
{
    public class ContentConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Question question)
            {
                return question.ContentType switch
                {
                    ContentType.Text => Encoding.UTF8.GetString(question.RawContent),
                    ContentType.Image => ByteArrayToBitmapImageConverter.Convert(question.RawContent),
                    ContentType.Video => question.ContentPath,
                    ContentType.Sound => question.ContentPath,
                    _ => null
                };
            }

            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}

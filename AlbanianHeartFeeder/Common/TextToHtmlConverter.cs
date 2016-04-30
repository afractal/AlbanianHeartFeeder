using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Html;
using Windows.UI.Xaml.Data;

namespace AlbanianHeartFeeder.Common
{
    public sealed class TextToHtmlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string)
            {
                return HtmlUtilities.ConvertToText(value.ToString());
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    // namespace ends here
}
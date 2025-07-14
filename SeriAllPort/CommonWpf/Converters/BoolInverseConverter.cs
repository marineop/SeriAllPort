using System.Globalization;
using System.Windows.Data;

namespace CommonWpf.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class BoolInverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool answer;
            if (value is bool originalValue)
            {
                answer = !originalValue;
            }
            else
            {
                answer = false;
            }

            return answer;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool answer;
            if (value is bool originalValue)
            {
                answer = !originalValue;
            }
            else
            {
                answer = false;
            }

            return answer;
        }
    }
}

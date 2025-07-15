using CommonWpf.Communication.Prococol;
using System.Globalization;
using System.Windows.Data;

namespace CommonWpf.Converters
{
    [ValueConversion(typeof(LengthMode), typeof(bool))]
    public class LengthModeToLengthEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool answer;
            if (value is LengthMode originalValue)
            {
                answer = originalValue == LengthMode.FixedLength;
            }
            else
            {
                answer = false;
            }

            return answer;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("Invalid Conversion");
        }
    }
}

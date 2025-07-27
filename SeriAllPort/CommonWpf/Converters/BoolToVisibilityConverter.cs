using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CommonWpf.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility answer;
            if (value is bool visible)
            {
                if (visible)
                {
                    answer = Visibility.Visible;
                }
                else
                {
                    answer = Visibility.Collapsed;
                }    
            }
            else
            {
                answer = Visibility.Visible;
            }

            return answer;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("Invalid conversion");
        }
    }
}

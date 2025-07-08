using CommonWpf.Communication;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CommonWpf.Converters
{
    [ValueConversion(typeof(ConnectionState), typeof(Brush))]
    public class ConnectionStateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ConnectionState connectionState = (ConnectionState)value;
            Brush answer;
            if (connectionState == ConnectionState.Connected)
            {
                answer = new SolidColorBrush(Colors.LightGreen);
            }
            else
            {
                answer = new SolidColorBrush(Colors.LightGray);
            }

            return answer;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Invalid Conversion");
        }
    }
}

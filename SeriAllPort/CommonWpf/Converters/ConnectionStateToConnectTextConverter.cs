using CommonWpf.Communication;
using System.Globalization;
using System.Windows.Data;

namespace CommonWpf.Converters
{
    [ValueConversion(typeof(ConnectionState), typeof(string))]
    public class ConnectionStateToConnectTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ConnectionState connectionState = (ConnectionState)value;
            string answer;
            if (connectionState == ConnectionState.Connected)
            {
                answer = "Disconnect";
            }
            else if (connectionState == ConnectionState.Disconnected)
            {
                answer = "Connect";
            }
            else
            {
                answer = "...";
            }

            return answer;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Invalid Conversion");
        }
    }
}

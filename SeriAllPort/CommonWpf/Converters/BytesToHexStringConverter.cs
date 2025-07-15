using CommonWpf.Extensions;
using System.Globalization;
using System.Windows.Data;

namespace CommonWpf.Converters
{
    [ValueConversion(typeof(byte[]), typeof(string))]
    public class BytesToHexStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string answer;
            if (value is not byte[] bytes)
            {
                answer = string.Empty;
            }
            else
            {
                answer = bytes.BytesToString();
            }

            return answer;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is string hexBytesInString)
                {
                    byte[] answer = hexBytesInString.HexStringToBytes();

                    return answer;
                }
            }
            catch
            {
            }

            return null;
        }
    }
}

using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace CommonWpf.Converters
{
    [ValueConversion(typeof(byte[]), typeof(string))]
    public class BytesToUTF8StringConverter : IValueConverter
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
                answer = Encoding.UTF8.GetString(bytes);
            }

            return answer;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Invalid Conversion");
        }
    }
}

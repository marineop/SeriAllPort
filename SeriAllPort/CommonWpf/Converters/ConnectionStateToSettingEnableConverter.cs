﻿using CommonWpf.Communication;
using System.Globalization;
using System.Windows.Data;

namespace CommonWpf.Converters
{
    [ValueConversion(typeof(ConnectionState), typeof(bool))]
    public class ConnectionStateToSettingEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ConnectionState connectionState = (ConnectionState)value;
            bool answer = false;
            if (connectionState == ConnectionState.Disconnected)
            {
                answer = true;
            }

            return answer;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Invalid Conversion");
        }
    }
}

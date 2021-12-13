using IBL.BO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace PL.Converters
{
    class DroneStatusToBoolConverterForRelease : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DroneStatuses status = (DroneStatuses)value;
            var isMaintenance = status == DroneStatuses.Maintenance;
            var isDelivered = status == DroneStatuses.Delivered;
            if (parameter != null && parameter.ToString().Equals("visibility"))
                return isMaintenance ? Visibility.Collapsed : Visibility.Visible;
            if (parameter != null && parameter.ToString().Equals("visibilityForCharging"))
                return !isDelivered?  Visibility.Visible: Visibility.Collapsed;           
            return isMaintenance;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

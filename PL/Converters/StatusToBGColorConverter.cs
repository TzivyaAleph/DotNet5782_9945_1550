using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using BO;

namespace PL.Converters
{
    internal class StatusToBGColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value == null ? Brushes.White : (DroneStatuses)value switch
            {
                DroneStatuses.Available => Brushes.Orange,
                DroneStatuses.Delivered => Brushes.Beige,
                DroneStatuses.Maintenance => Brushes.BlueViolet,
                _ => Brushes.White
            };

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Przypominajka_3._0
{
    class NameToNameConverter : IValueConverter
    {
        int input;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            input = int.Parse(value.ToString());
            if (input > 1)
                return value;
            if (input > 0)
                return "Tomorrow";
            if (input > -1)
                return "Today";
            return "Expired";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

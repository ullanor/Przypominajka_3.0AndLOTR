using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Przypominajka_3._0
{
    class NameToBrushConverterRates : IValueConverter
    {
        int input;
        //char[] testArray;
        //int r = 0;
        //string number = string.Empty;
        //float result = 0;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            input = int.Parse(value.ToString());
            if (input > 7)
                return Brushes.LightGreen;
            if (input > 3)
                return Brushes.YellowGreen;
            if (input > 0)
                return Brushes.Orange;
            if (input < 0)
                return Brushes.Brown;
            return Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

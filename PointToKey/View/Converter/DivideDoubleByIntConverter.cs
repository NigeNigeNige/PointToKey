using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PointToKey.View.Converter
{
    class DivideDoubleByIntConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var value0 = System.Convert.ToDouble(values[0]);
            if (double.IsNaN(value0))
            {
                return 0.0;
            }
            
            var value1 = System.Convert.ToInt32(values[1]);

            return value0 / value1;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

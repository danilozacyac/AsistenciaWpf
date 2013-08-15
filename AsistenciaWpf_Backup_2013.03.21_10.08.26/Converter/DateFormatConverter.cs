using System;
using System.Linq;
using System.Windows.Data;

namespace AsistenciaWpf.Converter
{
    public class DateFormatConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              System.Globalization.CultureInfo culture)
        {
            if (value is DateTime)
            {
                string format = "dd/MM/yyyy";

                return ((DateTime)value).ToString(format);

            }
            else
            {
                return " ";
            }

        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}

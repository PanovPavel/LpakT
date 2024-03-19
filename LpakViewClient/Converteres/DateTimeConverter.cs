using System;
using System.Globalization;
using System.Windows.Data;

namespace LpakViewClient
{
    /// <summary>
    /// Конвертор для типа <see cref="DateTime"/> в <see cref="string"/>
    /// </summary>
    public class DateTimeConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime && dateTime == DateTime.MinValue)
            {
                return null;
            }
            else if (value is DateTime date)
            {
                return date.ToString("dd.MM.yyyy");
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string dateString)
            {
                if (DateTime.TryParseExact(dateString, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }
            }
            throw new ArgumentException("not a valid date string format \"dd.MM.yyyy\". Impossible to convert this string in DateTime");
        }
    }
}
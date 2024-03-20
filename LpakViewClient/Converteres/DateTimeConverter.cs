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
        /// <summary>
        /// Конвертирует <see cref="DateTime"/> в <see cref="string"/> формата "dd.MM.yyyy".
        /// </summary>
        /// <returns>Возвращает null - Если дата MinValue. ВОзвращает если объект DateTime дату в виде строки "dd.MM.yyyy"</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case DateTime dateTime when dateTime == DateTime.MinValue:
                    return null;
                case DateTime date:
                    return date.ToString("dd.MM.yyyy");
                default:
                    return value;
            }
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
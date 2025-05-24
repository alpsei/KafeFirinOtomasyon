using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace KafeFirinMaui.Helpers
{
    public class FavoriteToClassIdConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isFavorite = value is bool b && b;
            return isFavorite ? "filled" : "empty";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

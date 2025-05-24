using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace KafeFirinMaui.Helpers
{
    public class FavoriteToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isFavorite = value is bool b && b;
            return isFavorite ? "filled_heart.png" : "heart.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

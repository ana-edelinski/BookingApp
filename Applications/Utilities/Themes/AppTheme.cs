using System;
using System.Windows;
namespace BookingApp.Applications.Utilities.Themes
{
    internal class AppTheme
    {
        public static void ChangeTheme(Uri themeuri)
        {
            ResourceDictionary Theme = new ResourceDictionary() { Source = themeuri };

            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(Theme);

        }
    }
}

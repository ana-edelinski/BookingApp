using BookingApp.Applications.Utilities.Localization;
using QuestPDF.Infrastructure;
using System.Windows;

namespace BookingApp
{
    public partial class App : Application
    {
        public void ChangeLanguage(string lang)
        {
            TranslationSource.Instance.CurrentCulture = new System.Globalization.CultureInfo(lang);
        }
    }
}
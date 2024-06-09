using BookingApp.Applications.Utilities.Themes;
using BookingApp.WPF.DTOs;
using System;
using System.Windows;
using System.Windows.Controls;

namespace BookingApp.View.OwnerView
{
    public partial class Settings : Page
    {
        private App app;
        private const string SRB = "sr-RS";
        private const string ENG = "en-US";
        public Settings()
        {
            InitializeComponent();
            app = (App)Application.Current;
        }

        private void LightTheme_Click(object sender, RoutedEventArgs e)
        {
            AppTheme.ChangeTheme(new Uri("Applications/Utilities/Themes/Default.xaml", UriKind.Relative));
            
        }

        private void DarkTheme_Click(object sender, RoutedEventArgs e)
        { 
            AppTheme.ChangeTheme(new Uri("Applications/Utilities/Themes/Dark.xaml", UriKind.Relative));

        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            ComboBoxItem selectedItem = comboBox.SelectedItem as ComboBoxItem;

            if (selectedItem != null)
            {
                if (selectedItem.Content.ToString().Equals("Srpski"))
                {
                    app.ChangeLanguage(SRB);
                }
                else
                {
                    app.ChangeLanguage(ENG);
                }

            }
            
        }
    }
}

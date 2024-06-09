using System.Windows;
using BookingApp.WPF.DTOs;
using BookingApp.WPF.ViewModel.OwnerViewModel;
using QuestPDF.Infrastructure;

namespace BookingApp.View.OwnerView
{

    public partial class OwnerOverview : Window
    {
        private readonly UserDTO user = new();
        public OwnerOverview(UserDTO user)
        {
            InitializeComponent();
            DataContext = new OwnerOverviewViewModel(user, this.OwnerOverviewFrame.NavigationService);
            QuestPDF.Settings.License = LicenseType.Community;
            this.user = user;
        }
        private void OpenProfileMenu_Click(object sender, RoutedEventArgs e)
        {
            ProfileMenu.IsOpen = true;
        }

        private void AccountInfo_Click(object sender, RoutedEventArgs e)
        {
            OwnerOverviewFrame.NavigationService.Navigate(new AccountInfo(user));
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            OwnerOverviewFrame.NavigationService.Navigate(new Settings());
        }
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new();
            signInForm.Show();
            Close(); 
        }
    }
}

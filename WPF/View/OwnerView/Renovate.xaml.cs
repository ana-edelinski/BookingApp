using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using BookingApp.WPF.DTOs;
using BookingApp.WPF.ViewModel.OwnerViewModel;

namespace BookingApp.View.OwnerView
{
    public partial class Renovate : Page
    {
        public Renovate(UserDTO user, int accommodationId, NavigationService navigationService)
        {
            InitializeComponent();
            DataContext = new RenovateViewModel(accommodationId, navigationService, user);
        }
    }
}

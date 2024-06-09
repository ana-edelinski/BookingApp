using BookingApp.WPF.DTOs;
using BookingApp.WPF.ViewModel.OwnerViewModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.View.OwnerView
{

    public partial class Accommodations : Page
    {
        public Accommodations(UserDTO user, NavigationService navigationService)
        {
            InitializeComponent();
            this.DataContext = new AccommodationsViewModel(navigationService, user);
        }
    }
}

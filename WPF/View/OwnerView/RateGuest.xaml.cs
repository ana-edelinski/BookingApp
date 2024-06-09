using BookingApp.WPF.DTOs;
using BookingApp.WPF.ViewModel.OwnerViewModel;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.View.OwnerView
{
    public partial class RateGuest : Page
    {
        public RateGuest(UserDTO user, GuestRatingDTO selectedRating, UserDTO selectedGuest, NavigationService navigationService)
        {
            InitializeComponent();
            DataContext = new RateGuestViewModel(user, selectedRating, selectedGuest, navigationService);
        }
    }
}

using BookingApp.WPF.DTOs;
using BookingApp.WPF.ViewModel.OwnerViewModel;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.View.OwnerView
{
    public partial class UnratedGuests : Page
    {
        public UnratedGuests(UserDTO user, NavigationService navigationService)
        {
            InitializeComponent();
            DataContext = new UnratedGuestsViewModel(user,navigationService);
        }
    }
}

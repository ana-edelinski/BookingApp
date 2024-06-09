using BookingApp.WPF.DTOs;
using BookingApp.WPF.ViewModel.OwnerViewModel;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.View.OwnerView
{

    public partial class AccommodationInfo : Page
    {
        public AccommodationInfo(UserDTO user, AccommodationDTO selectedAccommodation, NavigationService navigationService)
        {
            InitializeComponent();
            DataContext = new AccommodationInfoViewModel(navigationService,selectedAccommodation, user);
        }
    }
}

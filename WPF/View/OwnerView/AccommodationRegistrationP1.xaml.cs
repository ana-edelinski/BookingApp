using BookingApp.WPF.DTOs;
using BookingApp.WPF.ViewModel.OwnerViewModel;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.View.OwnerView
{

    public partial class AccommodationRegistrationP1 : Page
    {
        public AccommodationRegistrationP1(AccommodationDTO accommodationDTO, UserDTO userDTO, ObservableCollection<ImageDTO> imagesDTO, NavigationService navigationService)
        {
            InitializeComponent();
            DataContext = new AccommodationRegistrationViewModel1(accommodationDTO, userDTO, imagesDTO, navigationService);
        }
    }
}

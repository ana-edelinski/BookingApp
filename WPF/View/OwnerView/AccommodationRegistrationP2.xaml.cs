using BookingApp.WPF.DTOs;
using BookingApp.WPF.ViewModel.OwnerViewModel;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.View.OwnerView
{
    public partial class AccommodationRegistrationP2 : Page
    {
        public AccommodationRegistrationP2(AccommodationDTO accommodationDTO, UserDTO userDTO, ObservableCollection<ImageDTO> imagesDTO, NavigationService navigationService)
        {
            InitializeComponent();
            DataContext = new AccommodationRegistrationViewModel2(accommodationDTO,userDTO,imagesDTO,navigationService);
        }
    }
}

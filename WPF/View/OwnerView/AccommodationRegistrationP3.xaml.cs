using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Navigation;
using BookingApp.WPF.DTOs;
using BookingApp.WPF.ViewModel.OwnerViewModel;
namespace BookingApp.View.OwnerView
{

    public partial class AccommodationRegistrationP3 : Page
    { 
        public AccommodationRegistrationP3(AccommodationDTO accommodationDTO, UserDTO user, ObservableCollection<ImageDTO> images, NavigationService navigationService)
        {
            InitializeComponent();
            DataContext = new AccommodationRegistrationViewModel3(accommodationDTO,user,images,navigationService);
        }
    }
}

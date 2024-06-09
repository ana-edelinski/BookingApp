using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model.Enums;
using BookingApp.View.OwnerView;
using BookingApp.WPF.DTOs;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.OwnerViewModel
{
    public class AccommodationRegistrationViewModel1 : NotifyPropertyChangedImplemented
    {
        public ICommand HouseClick { get; }
        public ICommand ApartmentClick { get; }
        public ICommand CottageClick { get; }
        public ICommand NextPage {  get; }

        private NavigationService Navigate {  get; set; }
        private ObservableCollection<ImageDTO> Images { get; set; }
        private UserDTO User { get; set; }
        private bool hasType = false;
        public bool HasType 
        {
            get { return hasType; }
            set
            {
                if(hasType != value)
                {
                    hasType = value;
                    OnPropertyChanged(nameof(HasType));
                }
            } 
        }

        private AccommodationDTO accommodationDTO;
        public AccommodationDTO AccommodationDTO
        {
            get { return accommodationDTO; }
            set
            {
                if (accommodationDTO != value)
                {
                    accommodationDTO = value;
                    OnPropertyChanged(nameof(AccommodationDTO));
                }
            }
        }

        private void ExecuteHouseClick(object paremeter)
        {
            AccommodationDTO.Type = (AccommodationType)1;
            AccommodationDTO.HasType = true;
            Navigate.Navigate(new AccommodationRegistrationP2(AccommodationDTO, User, Images, Navigate));
        }

        private void ExecuteApartmentClick(object parameter)
        {
            AccommodationDTO.Type = 0;
            AccommodationDTO.HasType = true;
            Navigate.Navigate(new AccommodationRegistrationP2(AccommodationDTO, User, Images, Navigate));
        }

        private void ExecuteCottageClick(object parameter)
        {
            AccommodationDTO.Type = (AccommodationType)2;
            AccommodationDTO.HasType = true;
            Navigate.Navigate(new AccommodationRegistrationP2(AccommodationDTO, User, Images, Navigate));
        }

        private void ExecuteNextPageCommand(object parameter)
        {
            Navigate.Navigate(new AccommodationRegistrationP2(AccommodationDTO,User,Images, Navigate));
        }
        public AccommodationRegistrationViewModel1(AccommodationDTO accommodationDTO,UserDTO user, ObservableCollection<ImageDTO> images, NavigationService navigationService)
        {
            AccommodationDTO = accommodationDTO;
            User = user;
            Images = images;
            Navigate = navigationService;

            HouseClick = new RelayCommand(ExecuteHouseClick);
            ApartmentClick = new RelayCommand(ExecuteApartmentClick);
            CottageClick = new RelayCommand(ExecuteCottageClick);
            NextPage = new RelayCommand(ExecuteNextPageCommand);
        }
    }
}

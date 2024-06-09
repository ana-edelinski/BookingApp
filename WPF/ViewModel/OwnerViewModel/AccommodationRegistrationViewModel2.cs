using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.View.OwnerView;
using BookingApp.WPF.DTOs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.OwnerViewModel
{
    public class AccommodationRegistrationViewModel2 : NotifyPropertyChangedImplemented
    {
        private readonly LocationService locationService = new();

        public ICommand PreviousPageCommand { get; private set; }
        public ICommand NextPageCommand { get; private set; }
        private UserDTO User {  get; set; }
        private ObservableCollection<ImageDTO> Images {  get; set; }
        private NavigationService Navigate {  get; set; }

        private AccommodationDTO accommodationDTO;
        public AccommodationDTO AccommodationDTO
        {
            get { return accommodationDTO; }
            set
            {
                if (accommodationDTO != value)
                {
                    accommodationDTO = value;
                    SetSelectedLocation();
                    OnPropertyChanged(nameof(AccommodationDTO));
                }
            }
        }

        private ObservableCollection<string> comboBoxLocations;
        public ObservableCollection<string> ComboBoxLocations
        {
            get { return comboBoxLocations; }
            set
            {

                comboBoxLocations = value;
                OnPropertyChanged(nameof(ComboBoxLocations));
            }
        }

        private string textBoxSearch;
        public string TextBoxSearch
        {
            get { return textBoxSearch; }
            set
            {
                textBoxSearch = value;
                try
                {
                    UpdateComboBoxItems(value);
                }
                catch (Exception e)
                {

                }

                OnPropertyChanged(nameof(TextBoxSearch));
            }
        }

        private bool isDropDownOpen;
        public bool IsDropDownOpen
        {
            get { return isDropDownOpen; }
            set
            {
                if (isDropDownOpen != value)
                {
                    isDropDownOpen = value;
                    OnPropertyChanged(nameof(IsDropDownOpen));
                }
            }
        }

        private string selectedLocation;
        public string SelectedLocation
        {
            get { return selectedLocation; }
            set
            {
                if (selectedLocation != value)
                {
                    selectedLocation = value;
                    SetLocation();
                    OnPropertyChanged(nameof(SelectedLocation));
                }
            }
        }

        private void UpdateComboBoxItems(string searchText)
        {
            searchText = searchText.ToLower();
            ComboBoxLocations.Clear();

            foreach (LocationDTO location in locationService.GetAllDTO().
                                                        Where(location =>
                                                        location.City.ToLower().StartsWith(searchText) ||
                                                        location.Country.ToLower().StartsWith(searchText)).ToList())
            {
                comboBoxLocations.Add(location.City + ", " + location.Country);

            }
            IsDropDownOpen = true;
            if (comboBoxLocations.Count == 0) IsDropDownOpen = false;
        }

        private void SetLocation()
        {
            if (SelectedLocation != null)
            {
                AccommodationDTO.Location.City = SelectedLocation.Split(", ")[0];
                AccommodationDTO.Location.Country = SelectedLocation.Split(", ")[1];
                AccommodationDTO.LocationId = locationService.GetId(AccommodationDTO.Location);

            }
        }

        private void SetSelectedLocation()
        {
            if(AccommodationDTO.Location.Country != "")
            {
                TextBoxSearch = AccommodationDTO.Location.City + ", " + AccommodationDTO.Location.Country;
            }
        }

        private void ExecutePreviousPageCommand(object sender)
        {
            Navigate.Navigate(new AccommodationRegistrationP1(AccommodationDTO,User,Images,Navigate));
        }

        private void ExecuteNextPageCommand(object sender)
        {
            AccommodationDTO.Validate();
            if(AccommodationDTO.IsValid) Navigate.Navigate(new AccommodationRegistrationP3(AccommodationDTO, User, Images, Navigate));
        }
        public AccommodationRegistrationViewModel2(AccommodationDTO accommodationDTO, UserDTO user, ObservableCollection<ImageDTO> images, NavigationService navigationService)
        {
            ComboBoxLocations = new();
            AccommodationDTO = accommodationDTO;
            AccommodationDTO.CurrentPage = 2;
            User = user;
            Images = images;
            Navigate = navigationService;

            PreviousPageCommand = new RelayCommand(ExecutePreviousPageCommand);
            NextPageCommand = new RelayCommand(ExecuteNextPageCommand);
        }
    }
}

using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.View.OwnerView;
using BookingApp.WPF.DTOs;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.OwnerViewModel
{
    public class AccommodationsViewModel : NotifyPropertyChangedImplemented
    {
        private readonly AccommodationService accommodationService = new();
        private readonly ImageService imageService = new();

        public ICommand RegisterCommand { get; private set; }
        private UserDTO User {  get; set; }
        private NavigationService Navigate {  get; set; }
        private IEnumerable<dynamic> accommodationsDataGrid;
        public IEnumerable<dynamic> AccommodationsDataGrid
        {
            get {  return accommodationsDataGrid; }
            set
            {
                if(accommodationsDataGrid != value)
                {
                    accommodationsDataGrid = value;
                    
                    OnPropertyChanged(nameof(accommodationsDataGrid));
                }
            }
        }

        private dynamic selectedItem;
        public dynamic SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                    SelectionChanged();
                    OnPropertyChanged(nameof(SelectedItem));
                }
            }
        }

        private AccommodationDTO selectedAccommodation;
        public AccommodationDTO SelectedAccommodation
        {
            get { return selectedAccommodation; }
            set
            {
                if(selectedAccommodation != value)
                {
                    selectedAccommodation = value;
                    OnPropertyChanged(nameof(SelectedAccommodation));
                }
            }
        }

        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                if (searchText != value)
                {
                    searchText = value;
                    GetAccommodationsDataGrid();
                    OnPropertyChanged(nameof(SearchText));
                }
            }
        }

        private void SelectionChanged()
        {
            if (SelectedItem != null)
            {
                
                SelectedAccommodation = (AccommodationDTO)(SelectedItem.GetType().GetProperty("Accommodation")?.GetValue(SelectedItem, null));
                Navigate.Navigate(new AccommodationInfo(User, SelectedAccommodation, Navigate));
            }
        }

        private IEnumerable<dynamic> FilterData(IEnumerable<dynamic> data)
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                data = data.Where(data =>
                    data.Accommodation.Name.ToLower().Contains(SearchText.ToLower())
                );
            }

            return data;
        }

        private void GetAccommodationsDataGrid()
        {
            var joinedData = from accommodation in accommodationService.GetAllForOwner(User.Id)
                             join image in imageService.BindImageToAccommodation() on accommodation.Id equals image.AccommodationId
                             where User.Id == accommodation.OwnerId
                             select new
                             {
                                 Accommodation = accommodation,
                                 Image = image
                             };
            
            AccommodationsDataGrid = FilterData(joinedData);
        }

        private void ExecuteRegisterCommand(object parameter)
        {
            Navigate.Navigate(new AccommodationRegistrationP1(new AccommodationDTO(), User, new ObservableCollection<ImageDTO>(), Navigate));
        }
        public AccommodationsViewModel(NavigationService navigationService, UserDTO user)
        {
            Navigate = navigationService;
            User = user;

            RegisterCommand = new RelayCommand(ExecuteRegisterCommand);
        }
    }
}

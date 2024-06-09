using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.View.OwnerView;
using BookingApp.WPF.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.OwnerViewModel
{
    public class UnratedGuestsViewModel : NotifyPropertyChangedImplemented
    {
        private readonly GuestRatingService guestRatingService = new ();
        private readonly AccommodationService accommodationService = new();
        private readonly AccommodationReservationService accommodationReservationService = new();
        private readonly ImageService imageService = new();

        public ICommand RateGuestCommand { get; set; }
        private UserDTO User {  get; set; }
        private NavigationService Navigate {  get; set; }

        private IEnumerable<dynamic> unratedGuests;
        public IEnumerable<dynamic> UnratedGuests
        {
            get { return unratedGuests; }
            set
            {
                if(value != unratedGuests)
                {
                    unratedGuests = value;
                    OnPropertyChanged(nameof(UnratedGuests));
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

        private GuestRatingDTO selectedGuestRating;
        public GuestRatingDTO SelectedGuestRating
        {
            get { return selectedGuestRating; }
            set
            {
                if(selectedGuestRating != value)
                {
                    selectedGuestRating = value;
                    OnPropertyChanged(nameof(SelectedGuestRating));
                }
            }
        }

        private UserDTO selectedGuest;
        public UserDTO SelectedGuest
        {
            get { return selectedGuest; }
            set
            {
                if(selectedGuest != value)
                {
                    selectedGuest = value;
                    OnPropertyChanged(nameof(SelectedGuest));
                }
            }
        }

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        private void SelectionChanged()
        {
            if (SelectedItem != null)
            {

                SelectedGuestRating = (GuestRatingDTO)(SelectedItem.GetType().GetProperty("Rating")?.GetValue(SelectedItem, null)); 
                SelectedGuest = (UserDTO)(SelectedItem.GetType().GetProperty("UnratedGuest")?.GetValue(SelectedItem, null));

                IsSelected = true;
            }
        }

        public void GetUnratedGuestsDataGrid(UserDTO owner)
        {

            var combinedDataList = from accommodation in accommodationService.GetAllDTO()
                                   join reservation in accommodationReservationService.GetAll() on accommodation.Id equals reservation.AccommodationId
                                   join unratedGuest in guestRatingService.GetUnratedGuests() on reservation.GuestId equals unratedGuest.Id
                                   join image in imageService.BindImageToAccommodation() on accommodation.Id equals image.AccommodationId
                                   join rating in guestRatingService.GetAllDTO() on reservation.Id equals rating.AccommodationReservationId
                                   where owner.Id == accommodation.OwnerId && !rating.IsRated
                                   select new
                                   {
                                       Accommodation = accommodation,
                                       AccommodationReservation = reservation,
                                       UnratedGuest = unratedGuest,
                                       Image = image,
                                       Rating = rating
                                   };

            UnratedGuests = combinedDataList;
        }

        private void ExecuteRateGuestCommand(object parameter)
        {
            Navigate.Navigate(new RateGuest(User, SelectedGuestRating,SelectedGuest, Navigate));
        }
        public UnratedGuestsViewModel(UserDTO user, NavigationService navigationService) 
        {
            User = user;
            Navigate = navigationService;

            IsSelected = false;
            GetUnratedGuestsDataGrid(user);

            RateGuestCommand = new RelayCommand(ExecuteRateGuestCommand);
        }
    }
}

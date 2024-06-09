using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model;
using BookingApp.WPF.DTOs;
using BookingApp.WPF.View.GuestView;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.GuestViewModel
{
    public class RecentStaysViewModel : INotifyPropertyChanged
    {
        private readonly AccommodationService _accommodationService;
        private readonly UserService _userService;
        private readonly OwnerAccommodationRatingService _ownerAccommodationRatingService;
        private readonly AccommodationReservationService _accommodationReservationService;
        private readonly ImageService _imageService = new();
        private readonly UserDTO user;

        public ObservableCollection<AccommodationDTO> AllAccommodations { get; set; }
        public OwnerAccommodationRatingDTO SelectedUnratedAccommodation { get; set; }

        public RelayCommand RateRecommendCommand { get; set; }
        private NavigationService Navigate { get; set; }



        private ObservableCollection<OwnerAccommodationRatingDTO> _unratedAccommodations;
        public ObservableCollection<OwnerAccommodationRatingDTO> UnratedAccommodations
        {
            get => _unratedAccommodations;
            set
            {
                if (_unratedAccommodations != value)
                {
                    _unratedAccommodations = value;
                    OnPropertyChanged();
                }
            }
        }

        
        public RecentStaysViewModel(UserDTO user, NavigationService navigationService)
        {
            this.user = user;
            SelectedUnratedAccommodation = SelectedUnratedAccommodation;
            Navigate = navigationService;

            _accommodationService = new AccommodationService();
            _userService = new UserService();
            _accommodationReservationService = new AccommodationReservationService();

            _imageService = new ImageService();
            _ownerAccommodationRatingService = new OwnerAccommodationRatingService();
            AllAccommodations = new ObservableCollection<AccommodationDTO>(_accommodationService.GetAllDTO());


            UnratedAccommodations = new ObservableCollection<OwnerAccommodationRatingDTO>();
            FormUnratedReservation();

            RateRecommendCommand = new RelayCommand(Execute_RateRecommendCommand, CanExecute_Command);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public OwnerAccommodationRatingDTO ConvertToDTO(AccommodationReservationDTO reservation)
        {
            ImageDTO image = _imageService.GetOneForAccommodationDTO(reservation.AccommodationId);  
            string imagePath = image?.Path;
            var ownerId = _accommodationService.GetOwnerIdForAccommodation(reservation.AccommodationId);

            return new OwnerAccommodationRatingDTO(reservation.Id,
                _userService.GetById(ownerId).Username,
                _accommodationService.GetById(reservation.AccommodationId).Name,
                imagePath);
        }
        public ObservableCollection<OwnerAccommodationRatingDTO> ConvertToDTO(ObservableCollection<AccommodationReservationDTO> reservations)
        {
            ObservableCollection<OwnerAccommodationRatingDTO> dto = new ObservableCollection<OwnerAccommodationRatingDTO>();
            foreach (AccommodationReservationDTO reservation in reservations)
            {
                dto.Add(ConvertToDTO(reservation));
            }
            return dto;
        }

        private bool CanExecute_Command(object obj)
        {
            return true;
        }

        private void Execute_RateRecommendCommand(object parameter)
        {
            if (parameter is OwnerAccommodationRatingDTO selectedStay)
            {
                SelectedUnratedAccommodation = selectedStay;
                Navigate.Navigate(new RateRecommend(selectedStay, user));
            }
        }

        public void FormUnratedReservation()
        {
            UnratedAccommodations.Clear();
            var reservations = _ownerAccommodationRatingService.GetUnratedAccommodations().Where(r => RecentlyEnded(r));
            UnratedAccommodations = ConvertToDTO(new ObservableCollection<AccommodationReservationDTO>(reservations));
        }

        public bool RecentlyEnded(AccommodationReservationDTO reservation)
        {
            TimeSpan daysPassed = DateTime.Today - reservation.EndDate;
            return daysPassed.TotalDays >= 0 && daysPassed.TotalDays <= 5;
        }
    }
}

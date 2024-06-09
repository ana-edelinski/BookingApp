using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.WPF.DTOs;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookingApp.WPF.ViewModel.GuestViewModel
{
    public class RateRecommendViewModel : INotifyPropertyChanged
    {
        private readonly ImageService _imageService = new();
        private readonly OwnerAccommodationRatingService _ownerAccommodationRatingService = new();
        private readonly UserDTO user;


        private OwnerAccommodationRatingDTO _selectedUnratedAccommodation;
        public OwnerAccommodationRatingDTO SelectedUnratedAccommodation
        {
            get => _selectedUnratedAccommodation;
            set
            {
                if (_selectedUnratedAccommodation != value)
                {
                    _selectedUnratedAccommodation = value;
                    OnPropertyChanged(nameof(SelectedUnratedAccommodation));
                    OnPropertyChanged(nameof(AccommodationName));
                    OnPropertyChanged(nameof(OwnerName));
                    OnPropertyChanged(nameof(ImagePath));
                }
            }
        }

        public string AccommodationName
        {
            get => SelectedUnratedAccommodation?.AccommodationName;
        }
        public string OwnerName
        {
            get => SelectedUnratedAccommodation?.UserName;
        }

        public string ImagePath
        {
            get => SelectedUnratedAccommodation?.ImagePath;
        }
        
        public RateRecommendViewModel(OwnerAccommodationRatingDTO selectedUnratedAccommodation, UserDTO user)
        {
            SelectedUnratedAccommodation = selectedUnratedAccommodation;
            this.user = user;

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool CanExecute_Command(object obj)
        {
            return true;
        }

    }
}

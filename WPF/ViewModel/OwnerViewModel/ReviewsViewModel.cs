using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.WPF.DTOs;
using BookingApp.WPF.View.OwnerView;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.OwnerViewModel
{
    public class ReviewsViewModel : NotifyPropertyChangedImplemented
    {
        private readonly OwnerAccommodationRatingService ownerAccommodationRatingService = new ();
        private readonly AccommodationService accommodationService = new();
        private readonly AccommodationReservationService accommodationReservationService = new();
        private readonly UserService userService = new();
        private readonly ImageService imageService = new();

        public UserDTO OwnerDTO { get; set; }
        private NavigationService Navigate {  get; set; }

        private IEnumerable<dynamic> reviews;
        public IEnumerable<dynamic> Reviews
        {
            get { return reviews; }
            set
            {
                if(reviews != value)
                {
                    reviews = value;
                    OnPropertyChanged(nameof(Reviews));
                }
            }
        }

        private dynamic selectedItem;
        public dynamic SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if(selectedItem != value)
                {
                    selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));
                    Navigate.Navigate(new ReviewDetails(selectedItem, Navigate));
                }
            }
        }

        public void GetReviewsDataGrid(UserDTO owner)
        {

            var queryResult = from accommodation in accommodationService.GetAllDTO()
                              join reservation in accommodationReservationService.GetAllDTO() on accommodation.Id equals reservation.AccommodationId
                              join rating in ownerAccommodationRatingService.GetAllReviewsDTO(owner.Id) on reservation.Id equals rating.AccommodationReservationId
                              join user in userService.GetAllDTO() on reservation.GuestId equals user.Id
                              join image in imageService.BindImageToAccommodation() on accommodation.Id equals image.AccommodationId
                              select new
                              {
                                  Accommodation = accommodation,
                                  User = user,
                                  AccommodationReservation = reservation,
                                  OwnerAccommodationRating = rating,
                                  Image = image
                              };

            Reviews = queryResult;
        }


        public ReviewsViewModel(UserDTO user, NavigationService navigationService) 
        {
            OwnerDTO = user;
            Navigate = navigationService;

            GetReviewsDataGrid(user);
        }
    }
}

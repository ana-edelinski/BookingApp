using BookingApp.Applications.UseCases;
using BookingApp.WPF.DTOs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BookingApp.WPF.ViewModel.GuestViewModel
{
    public class MyReviewsViewModel : INotifyPropertyChanged
    {
        private readonly GuestRatingService _guestRatingService;
        private readonly UserService _userService;
        private readonly AccommodationReservationService _accommodationReservationService;
        private readonly CommentService _commentService;

        public UserDTO LoggedInUser { get; set; }
        public ObservableCollection<GuestRatingDTO> GuestRatings { get; set; }


        public MyReviewsViewModel(UserDTO user) 
        {
            LoggedInUser = user;

            GuestRatings = new ObservableCollection<GuestRatingDTO>();
            _guestRatingService = new GuestRatingService();
            _userService = new UserService();
            _accommodationReservationService = new AccommodationReservationService();
            _commentService = new CommentService();

            FormGuestRatings();

        }

        public void FormGuestRatings()
        {
            foreach (var guestRating in _guestRatingService.GetAll().Where(gr => gr.IsRated))
            {
                string ownerName = _accommodationReservationService.GetOwnerNameForAccommodationReservation(guestRating.AccommodationReservationId);
                string commentText = _commentService.GetCommentTextForGuestRating(guestRating.CommentId);
                GuestRatings.Add(new GuestRatingDTO(guestRating, ownerName, commentText));
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

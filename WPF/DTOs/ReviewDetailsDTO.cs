using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using System.Collections.ObjectModel;

namespace BookingApp.WPF.DTOs
{
    public class ReviewDetailsDTO : NotifyPropertyChangedImplemented
    {
        private readonly ImageService imageService = new();

        private ObservableCollection<System.Windows.Controls.Image> images;
        public ObservableCollection<System.Windows.Controls.Image> Images
        {
            get { return images; }
            set
            {
                if (images != value)
                {
                    images = value;

                    OnPropertyChanged(nameof(Images));
                }
            }
        }
        private ObservableCollection<ImageDTO> imagesDTO;
        public ObservableCollection<ImageDTO> ImagesDTO
        {
            get { return imagesDTO; }
            set
            {
                if (imagesDTO != value)
                {
                    imagesDTO = value;
                    OnPropertyChanged(nameof(ImagesDTO));
                }
            }
        }

        private UserDTO guest;
        public UserDTO Guest
        {
            get { return guest; }
            set
            {
                if (guest != value)
                {
                    guest = value;
                    OnPropertyChanged(nameof(Guest));
                }
            }
        }

        private AccommodationDTO accommodation;
        public AccommodationDTO Accommodation
        {
            get { return accommodation; }
            set
            {
                if (accommodation != value)
                {
                    accommodation = value;
                    OnPropertyChanged(nameof(Accommodation));
                }
            }
        }

        private AccommodationReservationDTO reservation;
        public AccommodationReservationDTO Reservation
        {
            get { return reservation; }
            set
            {
                if (reservation != value)
                {
                    reservation = value;
                    OnPropertyChanged(nameof(Reservation));
                }
            }
        }

        private OwnerAccommodationRatingDTO rating;
        public OwnerAccommodationRatingDTO Rating
        {
            get { return rating; }
            set
            {
                if (rating != value)
                {
                    rating = value;
                    ImagesDTO = imageService.GetAllForReview(Rating.Id);
                    OnPropertyChanged(nameof(Rating));
                }
            }
        }

        private CommentDTO comment;
        public CommentDTO Comment
        {
            get { return comment; }
            set
            {
                if (comment != value)
                {
                    comment = value;
                    OnPropertyChanged(nameof(Comment));
                }
            }
        }

        public ReviewDetailsDTO()
        {
            Images = new();
            ImagesDTO = new();
        }
    }
}

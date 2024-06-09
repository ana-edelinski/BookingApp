using Accessibility;
using BookingApp.Domain.Model;
using BookingApp.WPF.View.OwnerView;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BookingApp.WPF.DTOs
{
    public class OwnerAccommodationRatingDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }
        private int accommodationReservationId;
        public int AccommodationReservationId 
        {
            get { return accommodationReservationId; }

            set
            {
                if(accommodationReservationId != value)
                {
                    accommodationReservationId = value;
                    OnPropertyChanged(nameof(AccommodationReservationId));
                }
            }
        }

        private int commentId;
        public int CommentId 
        {
            get { return  commentId; }
            set
            {
                if(commentId != value)
                {
                    commentId = value;
                    OnPropertyChanged(nameof(CommentId));
                }
            }
        }

        private int ownerRating;
        public int OwnerRating 
        {
            get { return ownerRating; }
            set
            {
                if(ownerRating != value)
                {
                    ownerRating = value;
                    OnPropertyChanged(nameof(OwnerRating));
                }
            }
        }

        private int accommodationRating;
        public int AccommodationRating 
        {
            get { return accommodationRating; }
            set
            {
                if(accommodationRating != value)
                {
                    accommodationRating = value;
                    OnPropertyChanged(nameof(AccommodationRating));
                }
            }
        }

        private bool isRated;
        public bool IsRated 
        {
            get { return isRated; }
            set
            {
                if(isRated != value)
                {
                    isRated = value;
                    OnPropertyChanged(nameof(IsRated));
                }
            }
        }
        public double AverageRating
        {
            get
            {
                return CalculateAverage();
            }
        }


        public string UserName { get; set; }
        public string AccommodationName { get; set; }
        public string ImagePath { get; set; }


        public double CalculateAverage()
        {
            return (OwnerRating + AccommodationRating) / 2.0;
        }

        public OwnerAccommodationRatingDTO() 
        {
            
        }

        public OwnerAccommodationRatingDTO(int reservationId, string userName, string accommodationName, string imagePath)
        {
            AccommodationReservationId = reservationId;
            UserName = userName;
            AccommodationName = accommodationName;
            ImagePath = imagePath;
        }

        public OwnerAccommodationRatingDTO(OwnerAccommodationRating oar)
        {
            Id = oar.Id;
            OwnerRating = oar.OwnerRating;
            IsRated = oar.IsRated;
            AccommodationRating = oar.AccommodationRating;
            CommentId = oar.CommentId;
            AccommodationReservationId = oar.AccommodationReservationId;
            
        }

        public OwnerAccommodationRating ToOwnerAccommodationRating()
        {
            return new OwnerAccommodationRating(Id, OwnerRating, IsRated, AccommodationRating, CommentId, AccommodationReservationId);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

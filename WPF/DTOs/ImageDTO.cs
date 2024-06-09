using BookingApp.Applications.Utilities.Validation;
using BookingApp.Domain.Model;
using System.Collections.Generic;
using System.ComponentModel;

namespace BookingApp.WPF.DTOs
{
    public class ImageDTO : ValidationBase 
    {
        public int Id { get; set; }
        private string path;
        public string Path
        {
            get { return path; }
            set
            {
                if (path != value)
                {
                    path = value;
                    OnPropertyChanged(nameof(Path));
                }
            }
        }

        private string originPath;
        public string OriginPath
        {
            get { return originPath; }
            set
            {
                if (originPath != value)
                {
                    originPath = value;
                    OnPropertyChanged(nameof(OriginPath));
                }
            }
        }

        private int accommodationId;
        public int AccommodationId
        {
            get { return accommodationId; }
            set
            {
                if (accommodationId != value)
                {
                    accommodationId = value;
                    OnPropertyChanged(nameof(AccommodationId));
                }
            }
        }

        private int tourId;
        public int TourId
        {
            get { return tourId; }
            set
            {
                if (tourId != value)
                {
                    tourId = value;
                    OnPropertyChanged(nameof(TourId));
                }
            }
        }

        private int ownerAccommodationRatingId;
        public int OwnerAccommodationRatingId
        {
            get { return ownerAccommodationRatingId; }
            set
            {
                if (ownerAccommodationRatingId != value)
                {
                    ownerAccommodationRatingId = value;
                    OnPropertyChanged(nameof(OwnerAccommodationRatingId));
                }
            }
        }

        protected override void ValidateSelf()
        {

        }
        public Image ToImage()
        {
            return new Image(Id, Path, AccommodationId, TourId, OwnerAccommodationRatingId);
        }
        public ImageDTO(Image image)
        {
            Id = image.Id;
            Path = image.Path;
            AccommodationId = image.AccommodationId;
            TourId = image.TourId;
            OwnerAccommodationRatingId = image.OwnerAccommodationRatingId;
        }

        public ImageDTO()
        {
            AccommodationId = -1;
            TourId = -1;
            OwnerAccommodationRatingId = -1;
        }
    }
}

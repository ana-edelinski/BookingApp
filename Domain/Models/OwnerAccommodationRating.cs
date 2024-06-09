using BookingApp.Applications.UtilityInterfaces;
using System;

namespace BookingApp.Domain.Model
{
    public class OwnerAccommodationRating : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationReservationId { get; set; }
        public int CommentId { get; set; }
        public int OwnerRating { get; set; }
        public int AccommodationRating { get; set; }
        public bool IsRated { get; set; }
        public double AverageRating
        {
            get
            {
                return CalculateAverage();
            }
        }

        public OwnerAccommodationRating()
        {
            OwnerRating = 0;
            AccommodationRating = 0;
            CommentId = -1;
            IsRated = false;
        }

        public OwnerAccommodationRating(int id, int ownerRating, bool isRated, int accommodationRating, int commentId, int accommodationReservationId)
        {
            Id = id;
            OwnerRating = ownerRating;
            IsRated = isRated;
            AccommodationRating = accommodationRating;
            CommentId = commentId;
            AccommodationReservationId = accommodationReservationId;

        }

        public void FromCSV(string[] csvValues)
        {
            Id = Convert.ToInt32(csvValues[0]);
            AccommodationReservationId = Convert.ToInt32(csvValues[1]);
            OwnerRating = Convert.ToInt32(csvValues[2]);
            AccommodationRating = Convert.ToInt32(csvValues[3]);
            CommentId = Convert.ToInt32(csvValues[4]);
            IsRated = Convert.ToBoolean(csvValues[5]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationReservationId.ToString(),
                OwnerRating.ToString(),
                AccommodationRating.ToString(),
                CommentId.ToString(),
                IsRated.ToString(),
            };

            return csvValues;
        }

        public double CalculateAverage()
        {
            return (OwnerRating + AccommodationRating) / 2.0;
        }
    }
}

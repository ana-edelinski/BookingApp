using BookingApp.Applications.UtilityInterfaces;
using System;

namespace BookingApp.Domain.Model
{
    public class GuestRating : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationReservationId { get; set; }
        public int Cleanliness { get; set; }
        public int RuleRespect { get; set; }
        public int CommentId { get; set; }
        public bool IsRated { get; set; }
        public GuestRating()
        {
            Id = -1;
            Cleanliness = -1;
            RuleRespect = -1;
            CommentId = -1;
            IsRated = false;
        }

        public GuestRating(int id, int accommodationReservationId, int cleanliness, int ruleRespect, int commentId, bool isRated)
        {
            Id = id;
            AccommodationReservationId = accommodationReservationId;
            Cleanliness = cleanliness;
            RuleRespect = ruleRespect;
            CommentId = commentId;
            IsRated = isRated;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationReservationId.ToString(),
                Cleanliness.ToString(),
                RuleRespect.ToString(),
                CommentId.ToString(),
                IsRated.ToString(),
            };

            return csvValues;
        }

        public void FromCSV(string[] csvValues)
        {
            Id = Convert.ToInt32(csvValues[0]);
            AccommodationReservationId = Convert.ToInt32(csvValues[1]);
            Cleanliness = Convert.ToInt32(csvValues[2]);
            RuleRespect = Convert.ToInt32(csvValues[3]);
            CommentId = Convert.ToInt32(csvValues[4]);
            IsRated = Convert.ToBoolean(csvValues[5]);
        }
    }
}

using BookingApp.Applications.UtilityInterfaces;
using System;

namespace BookingApp.Domain.Models
{
    public class RenovationRecommendation : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public string Information { get; set; }
        public int EmergencyLevel { get; set; } 
        public int GuestId { get; set; }
        public DateTime CreationTime {  get; set; }

        public RenovationRecommendation() { }
        public RenovationRecommendation(/*int id,*/ int accommodationId, string information, int emergencyLevel, int guestId, DateTime creationTime)
        {
            // Id = id;
            AccommodationId = accommodationId;
            Information = information;
            EmergencyLevel = emergencyLevel;
            GuestId = guestId;
            CreationTime = creationTime;
        }

        
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationId = Convert.ToInt32(values[1]);
            Information = values[2];
            EmergencyLevel = Convert.ToInt32(values[3]);
            GuestId = Convert.ToInt32(values[4]);
            CreationTime = Convert.ToDateTime(values[5]);
        }

        public string[] ToCSV()
        {
            string[] values =
            {
                Id.ToString(),
                AccommodationId.ToString(),
                Information,
                EmergencyLevel.ToString(),
                GuestId.ToString(),
                CreationTime.ToString()
            };

            return values;
        }
    }
}

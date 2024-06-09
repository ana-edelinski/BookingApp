using BookingApp.Applications.UtilityInterfaces;
using System;


namespace BookingApp.Domain.Model
{
    public class Image : ISerializable
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int AccommodationId { get; set; }
        public int TourId { get; set; }
        public int OwnerAccommodationRatingId { get; set; }

        public Image()
        {
            Id = -1;
            Path = string.Empty;
            AccommodationId = -1;
            TourId = -1;
            OwnerAccommodationRatingId = -1;
        }

        public Image(int id, string path, int accommodationId, int tourId, int ownerAccommodationRatingId)
        {
            Id = id;
            Path = path;
            AccommodationId = accommodationId;
            TourId = tourId;
            OwnerAccommodationRatingId = ownerAccommodationRatingId;
        }

        public void FromCSV(string[] csvValues)
        {
            Id = Convert.ToInt32(csvValues[0]);
            Path = csvValues[1];
            AccommodationId = Convert.ToInt32(csvValues[2]);
            TourId = Convert.ToInt32(csvValues[3]);
            OwnerAccommodationRatingId = Convert.ToInt32(csvValues[4]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Path.ToString(),
                AccommodationId.ToString(),
                TourId.ToString(),
                OwnerAccommodationRatingId.ToString()
            };
            return csvValues;
        }
    }
}
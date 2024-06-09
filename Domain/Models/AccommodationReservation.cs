using BookingApp.Applications.UtilityInterfaces;
using System;

namespace BookingApp.Domain.Model
{
    public class AccommodationReservation : ISerializable
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public int AccommodationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberDays { get; set; }
        public bool IsAvailable { get; set; }
        public int Capacity { get; set; }  

        public bool IsCanceled {  get; set; }


        public AccommodationReservation()
        {

        }



        public AccommodationReservation(int id, int guestId, int accommodationId, DateTime startDate, DateTime endDate, int numberDays, bool isAvailable, int capacity, bool isCanceled)


        {
            Id = id;
            GuestId = guestId;
            AccommodationId = accommodationId;
            StartDate = startDate;
            EndDate = endDate;
            NumberDays = numberDays;
            IsAvailable = isAvailable;
            Capacity = capacity;
            IsCanceled = isCanceled;
        }

        public AccommodationReservation(int guestId, int accommodationId, DateTime startDate, DateTime endDate, int numberDays, int capacity)
        {
            GuestId = guestId;
            AccommodationId = accommodationId;
            StartDate = startDate;
            EndDate = endDate;
            NumberDays = numberDays;
            IsAvailable = false;
            Capacity = capacity;
        }

        public void FromCSV(string[] csvValues)
        {
            Id = int.Parse(csvValues[0]);
            AccommodationId = int.Parse(csvValues[1]);
            StartDate = DateTime.Parse(csvValues[2]);
            EndDate = DateTime.Parse(csvValues[3]);
            GuestId = int.Parse(csvValues[4]);
            NumberDays = Convert.ToInt32(csvValues[5]);
            Capacity = Convert.ToInt32(csvValues[6]);
            IsAvailable = Convert.ToBoolean(csvValues[7]);
            IsCanceled = Convert.ToBoolean(csvValues[8]);    
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationId.ToString(),
                StartDate.ToString(),
                EndDate.ToString(),
                GuestId.ToString(),
                NumberDays.ToString(),
                Capacity.ToString(),
                IsAvailable.ToString(),
                IsCanceled.ToString()
            };

            return csvValues;
        }

        public bool IsLessThan5Days()
        {
            
            DateTime today = DateTime.Today;

            DateTime endDateTime = EndDate;

            TimeSpan difference = today - endDateTime;
            double days = difference.TotalDays;

            return days >= 0 && days <= 5;
            
        }
    }
}

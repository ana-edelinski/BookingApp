using BookingApp.Applications.UtilityInterfaces;
using BookingApp.Domain.Model.Enums;
using System;
using System.Diagnostics;

namespace BookingApp.Domain.Model
{
    public class Accommodation : ISerializable
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public AccommodationType Type { get; set; }
        public int Capacity { get; set; }
        public int MinReservationDays { get; set; }
        public int CancelDaysPriorReservation { get; set; }
        public string ImagePath { get; set; }

        public Accommodation()
        {
            Location = new();
            MinReservationDays = 1;
        }

        public Accommodation(int id, string name, int ownerId, int locationId, Location location, AccommodationType type, int capacity, int minReservationDays, int cancelDaysPriorReservation, string imagePath)
        {
            Id = id;
            Name = name;
            OwnerId = ownerId;
            LocationId = locationId;
            Location = location;
            Type = type;
            Capacity = capacity;
            MinReservationDays = minReservationDays;
            CancelDaysPriorReservation = cancelDaysPriorReservation;
            ImagePath = imagePath;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            LocationId = int.Parse(values[2]);
            Type = (AccommodationType)Convert.ToInt32(values[3]);
            Capacity = Convert.ToInt32(values[4]);
            MinReservationDays = Convert.ToInt32(values[5]);
            CancelDaysPriorReservation = Convert.ToInt32(values[6]);
            OwnerId = Convert.ToInt32(values[7]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Name,
                LocationId.ToString(),
                Convert.ToInt32(Type).ToString(),
                Capacity.ToString(),
                MinReservationDays.ToString(),
                CancelDaysPriorReservation.ToString(),
                OwnerId.ToString(),
            };
            return csvValues;
        }

    }


}
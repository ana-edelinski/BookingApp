using BookingApp.Applications.Utilities.Validation;
using BookingApp.Domain.Model;
using BookingApp.Domain.Model.Enums;
using System.Collections.Generic;

namespace BookingApp.WPF.DTOs
{
    public class AccommodationDTO : ValidationBase
    {
        public int Id { get; set; }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private int ownerId;
        public int OwnerId
        {
            get { return ownerId; }
            set
            {
                if (ownerId != value)
                {
                    ownerId = value;
                    OnPropertyChanged(nameof(OwnerId));
                }
            }
        }

        private int locationId;
        public int LocationId
        {
            get { return locationId; }
            set
            {
                if (locationId != value)
                {
                    locationId = value;
                    OnPropertyChanged(nameof(LocationId));
                }
            }
        }

        private Location location;
        public Location Location
        {
            get { return location; }
            set
            {
                if (location != value)
                {
                    location = value;
                    OnPropertyChanged(nameof(Location));
                }
            }
        }

        private AccommodationType type;
        public AccommodationType Type
        {
            get { return type; }
            set
            {
                if (type != value)
                {
                    type = value;
                    OnPropertyChanged(nameof(Type));
                }
            }
        }
        private int capacity;
        public int Capacity
        {
            get { return capacity; }
            set
            {
                if (capacity != value)
                {
                    capacity = value;
                    OnPropertyChanged(nameof(Capacity));
                }
            }
        }

        private int minReservationDays;
        public int MinReservationDays
        {
            get { return minReservationDays; }
            set
            {
                if (minReservationDays != value)
                {
                    minReservationDays = value;
                    OnPropertyChanged(nameof(MinReservationDays));
                }
            }
        }

        private int cancelDaysPriorReservation;
        public int CancelDaysPriorReservation
        {
            get { return cancelDaysPriorReservation; }
            set
            {
                if (cancelDaysPriorReservation != value)
                {
                    cancelDaysPriorReservation = value;
                    OnPropertyChanged(nameof(CancelDaysPriorReservation));
                }
            }
        }

        private bool hasType;
        public bool HasType
        {
            get { return hasType; }
            set
            {
                if (hasType != value)
                {
                    hasType = value;
                    OnPropertyChanged(nameof(HasType));
                }
            }
        }

        public string ImagePath { get; set; }
        public List<DateSpan> DateIntervals { get; set; }


        public Accommodation ToAccommodation()
        {
            return new Accommodation(Id, Name, OwnerId, LocationId, Location, Type, Capacity, MinReservationDays, CancelDaysPriorReservation, ImagePath);
        }

        public int CurrentPage;
        protected override void ValidateSelf()
        {
            switch (CurrentPage)
            {
                case 2:
                    if (string.IsNullOrWhiteSpace(this.name))
                    {
                        this.ValidationErrors["Name"] = "Name is required.";
                    }
                    if (string.IsNullOrEmpty(this.location.Country))
                    {
                        this.ValidationErrors["Location"] = "Please select a valid location.";
                    }
                    break;
                case 3:
                    if (Capacity < 1)
                    {
                        this.ValidationErrors["Capacity"] = "Capacity cannot be 0.";
                    }
                    if (MinReservationDays < 1)
                    {
                        this.ValidationErrors["MinReservationDays"] = "Minimum reservation days cannot be 0.";
                    }
                    if (CancelDaysPriorReservation < 1)
                    {
                        this.ValidationErrors["CancelDaysPriorReservation"] = "Cancel days prior reservation cannot be 0.";
                    }
                    break;
            }
            
        }

        public AccommodationDTO(Accommodation accommodation)
        {
            Id = accommodation.Id;
            OwnerId = accommodation.OwnerId;
            Name = accommodation.Name;
            LocationId = accommodation.LocationId;
            Location = accommodation.Location;
            Type = accommodation.Type;
            Capacity = accommodation.Capacity;
            MinReservationDays = accommodation.MinReservationDays;
            CancelDaysPriorReservation = accommodation.CancelDaysPriorReservation;
            ImagePath = accommodation.ImagePath;

            DateIntervals = new List<DateSpan>();


        }

        public AccommodationDTO()
        {
            Location = new();
            HasType = false;
        }
    }
}

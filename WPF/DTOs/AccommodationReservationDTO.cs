using BookingApp.Domain.Model;
using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Windows.Controls;

namespace BookingApp.WPF.DTOs
{
    public class AccommodationReservationDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }
        private int guestId;
        public int GuestId
        {
            get { return guestId; }
            set
            {
                if (guestId != value)
                {
                    guestId = value;
                    OnPropertyChanged(nameof(GuestId));
                }
            }
        }

        private int accommodationId;
        public int AccommodationId
        {
            get { return accommodationId; }
            set
            {
                if(accommodationId != value)
                {
                    accommodationId = value;
                    OnPropertyChanged(nameof(accommodationId));
                }
            }
        }

        private DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                if(startDate != value)
                {
                    startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }

        private DateTime endDate;
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                if(endDate != value)
                {
                    endDate = value;
                    OnPropertyChanged(nameof(EndDate)); 
                }
            }
        }

        private int numberDays;
        public int NumberDays
        {
            get => numberDays;
            set
            {
                if(numberDays != value)
                {
                    numberDays = value;
                    OnPropertyChanged(nameof(NumberDays));
                }
            }
        }

        private bool isAvailable;
        public bool IsAvailable
        {
            get => isAvailable;
            set
            {
                if(isAvailable != value)
                {
                    isAvailable = value;
                    OnPropertyChanged(nameof(IsAvailable));
                }
            }
        }

        private int capacity;
        public int Capacity
        {
            get => capacity;
            set
            {
                if (capacity != value)
                {
                    capacity = value;
                    OnPropertyChanged(nameof(Capacity));
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

        private bool isCanceled;
        public bool IsCanceled
        {
            get { return isCanceled; }
            set
            {
                if (isCanceled != value)
                {
                    isCanceled = value;
                    OnPropertyChanged(nameof(IsCanceled));
                }
            }
        }

        public string ImagePath { get; set; }
        public string AccommodationName { get; set; }

        public AccommodationReservation ToReservation()
        {
            return new AccommodationReservation(Id, GuestId, AccommodationId, StartDate, EndDate, NumberDays, IsAvailable, Capacity, IsCanceled);

        }
        public AccommodationReservationDTO() 
        { 
        
        }

        public AccommodationReservationDTO(AccommodationReservation accommodationReservation)
        {
            Id = accommodationReservation.Id;
            GuestId = accommodationReservation.GuestId;
            AccommodationId = accommodationReservation.AccommodationId;
            StartDate = accommodationReservation.StartDate;
            EndDate = accommodationReservation.EndDate;
            NumberDays = accommodationReservation.NumberDays;
            IsAvailable = accommodationReservation.IsAvailable;
            Capacity = accommodationReservation.Capacity;
            IsCanceled = accommodationReservation.IsCanceled;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

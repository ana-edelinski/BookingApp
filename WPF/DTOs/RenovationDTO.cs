using BookingApp.Domain.Models;
using System;
using System.ComponentModel;

namespace BookingApp.WPF.DTOs
{
    public class RenovationDTO : INotifyPropertyChanged
    {
        public int Id {  get; set; }
       
        private int accommodationId;
        public int AccommodationId
        {
            get { return accommodationId; }
            set
            {
                if(accommodationId != value)
                {
                    accommodationId = value;
                    OnPropertyChanged(nameof(AccommodationId));
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
                if (endDate != value)
                {
                    endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }

        public Renovation ToRenovation()
        {
            return new Renovation(Id,AccommodationId,StartDate,EndDate);
        }

        public RenovationDTO(int accommodationId, DateTime startDate, DateTime endDate)
        {
            AccommodationId = accommodationId;
            StartDate = startDate;
            EndDate = endDate;
        }

        public RenovationDTO(Renovation renovation)
        {
            Id = renovation.Id;
            AccommodationId = renovation.AccommodationId;
            StartDate = renovation.StartDate;
            EndDate = renovation.EndDate;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

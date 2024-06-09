using BookingApp.Domain.Models;
using System;
using System.ComponentModel;

namespace BookingApp.WPF.DTOs
{
    public class ForumDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private int locationId;
        public int LocationId
        {
            get { return locationId;}
            set
            {
                if(locationId != value)
                {
                    locationId = value;
                    OnPropertyChanged(nameof(LocationId));
                }
            }
        }

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

        private DateTime creationTime;
        public DateTime CreationTime
        {
            get { return creationTime; }
            set
            {
                if(creationTime != value)
                {
                    creationTime = value;
                    OnPropertyChanged(nameof(CreationTime));
                }
            }
        }

        private string country;
        public string Country
        {
            get { return country; }
            set
            {
                if (country != value)
                {
                    country = value;
                    OnPropertyChanged(nameof(Country));
                }
            }
        }

        private string city;
        public string City
        {
            get { return city; }
            set
            {
                if (city != value)
                {
                    city = value;
                    OnPropertyChanged(nameof(City));
                }
            }
        }

        private bool isOpened;
        public bool IsOpened
        {
            get { return isOpened;  }
            set
            {
                if(isOpened != value)
                {
                    isOpened = value;
                    OnPropertyChanged(nameof(IsOpened));
                }
            }
        }

        private bool isVeryUseful;
        public bool IsVeryUseful
        {
            get { return isVeryUseful; }
            set
            {
                if (isVeryUseful != value)
                {
                    isVeryUseful = value;
                    OnPropertyChanged(nameof(IsVeryUseful));
                }
            }
        }

        private int guestCommentsCount;
        public int GuestCommentsCount
        {
            get { return guestCommentsCount; }
            set
            {
                if (guestCommentsCount != value)
                {
                    guestCommentsCount = value;
                    OnPropertyChanged(nameof(GuestCommentsCount));
                }
            }
        }

        private int ownerCommentsCount;
        public int OwnerCommentsCount
        {
            get { return ownerCommentsCount; }
            set
            {
                if (ownerCommentsCount != value)
                {
                    ownerCommentsCount = value;
                    OnPropertyChanged(nameof(OwnerCommentsCount));
                }
            }
        }


        public bool IsNotification()
        {
            return (DateTime.Now - CreationTime).Days <= 1;
        }

        public Forum ToForum()
        {
            return new Forum(Id, LocationId, GuestId, CreationTime, IsOpened, GuestCommentsCount, OwnerCommentsCount, IsVeryUseful);
        }

        public ForumDTO FromForum(Forum forum)
        {
            return new ForumDTO
            {
                Id = forum.Id,
                LocationId = forum.LocationId,
                GuestId = forum.GuestId,
                CreationTime = forum.CreationTime,
                IsOpened = forum.IsOpened,
                IsVeryUseful = forum.IsVeryUseful,
                GuestCommentsCount = forum.GuestCommentsCount, // Dodajte ovo da bi preneli ažuriranu vrednost
                OwnerCommentsCount = forum.OwnerCommentsCount
            };
        }

        public ForumDTO(Forum forum)
        {
            Id = forum.Id;
            LocationId = forum.LocationId;
            GuestId = forum.GuestId;
            CreationTime = forum.CreationTime;
        }

        public ForumDTO() { }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

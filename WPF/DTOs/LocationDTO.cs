using BookingApp.Domain.Model;
using System.ComponentModel;

namespace BookingApp.WPF.DTOs
{
    public class LocationDTO : INotifyPropertyChanged
    {
        public int Id {  get; set; }
        private string city;
        public string City
        {
            get { return city; }
            set
            {
                if(city != value)
                {
                    city = value;
                    OnPropertyChanged(nameof(City));
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
        public LocationDTO(Location location) 
        {
            Id = location.Id;
            City = location.City;
            Country = location.Country;
        }

        public LocationDTO()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

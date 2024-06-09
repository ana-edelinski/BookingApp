using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Observer;

namespace BookingApp.Repository
{
    public class LocationRepository : ILocationRepository
    {
        private const string filePath = "../../../Resources/Data/locations.csv";
        private readonly Serializer<Location> _serializer;

        private readonly List<Location> locations;

        public LocationRepository()
        {
            _serializer = new Serializer<Location>();
            locations = _serializer.FromCSV(filePath);
        }

        private int NextId()
        {
            if(locations.Count == 0) return 0;
            return locations.Max(location => location.Id) + 1;
        }

        public List<Location> GetAll()
        {
            return locations;
        }

        public Location GetById(int id)
        {
            foreach (Location location in locations)
            {
                if (location.Id == id)
                {
                    return location;
                }
            }
            return null;
        }

        public Location Save(Location location)
        {
            location.Id = NextId();
            locations.Add(location);
            _serializer.ToCSV(filePath, locations);

            return location;
        }

        //remove all bellow
        public List<string> GetAllCountries()
        {
            List<string> countries = new List<string>();

            foreach (Location location in locations)
            {
                if (!countries.Contains(location.Country))
                {
                    countries.Add(location.Country);
                }
            }

            return countries;
        }

        public List<string> GetAllCities()
        {
            List<string> cities = new List<string>();

            foreach (Location location in locations)
            {
                cities.Add(location.City);
            }

            return cities;
        }

        public List<string> GetCitiesByCountry(string country)
        {
            List<string> cities = new List<string>();

            if (country == "Not Specified")
            {
                return GetAllCities();
            }

            foreach (Location location in locations)
            {
                if (location.Country == country)
                {
                    cities.Add(location.City);
                }
            }

            return cities;
        }



        public int GetId(Location location)
        {
            List<Location> locations = GetAll();
            Location? foundLocation = locations.Find(l => l.Country == location.Country && l.City == location.City);

            return foundLocation.Id;
        }


    }
}

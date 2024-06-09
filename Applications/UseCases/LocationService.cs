using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BookingApp.Applications.UseCases
{
    public class LocationService : ILocationRepository
    {
        private readonly ILocationRepository _repository;
        public LocationService()
        {
            _repository = Injector.CreateInstance<ILocationRepository>();
        }

        public List<LocationDTO> GetAllDTO()
        {
            List<LocationDTO> locations = new();
            foreach(Location location in GetAll())
            {
                locations.Add(new LocationDTO(location));
            }

            return locations;
        }

        public List<Location> GetAll()
        {
            return _repository.GetAll();
        }

        public Location GetById(int id)
        {
            return _repository.GetById(id);
        }

        public LocationDTO GetByIdDTO(int id) 
        {
            return new LocationDTO(GetById(id));
        }

        public Location Save(Location location)
        {
            return _repository.Save(location);
        }

        public List<string> GetAllCountries()
        {
            List<string> countries = new List<string>();

            foreach (Location location in GetAll())
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

            foreach (Location location in GetAll())
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

            foreach (Location location in GetAll())
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
            return GetAll().Find(l => l.Country == location.Country && l.City == location.City).Id;
        }

        public Tuple<LocationDTO, LocationDTO> GetRecommendationsForReservations()
        {
            AccommodationReservationService reservationService = new();
            int bestLocation = 1; //id
            int worstLocation = 1; //id


            foreach (LocationDTO location in GetAllDTO())
            {
                if (reservationService.GetNumberOfReservationsForLocation(bestLocation) < reservationService.GetNumberOfReservationsForLocation(location.Id)) bestLocation = location.Id;
                if (reservationService.GetNumberOfReservationsForLocation(worstLocation) > reservationService.GetNumberOfReservationsForLocation(location.Id)) worstLocation = location.Id;
            }

            return new Tuple<LocationDTO, LocationDTO>(GetByIdDTO(bestLocation), GetByIdDTO(worstLocation)); 
        }

        public Tuple<LocationDTO, LocationDTO> GetRecommendationsForOccupancy()
        {
            AccommodationReservationService reservationService = new();
            int bestLocation = 1; //id
            int worstLocation = 1; //id


            foreach (LocationDTO location in GetAllDTO())
            {
                if (reservationService.GetOccupancyForLocation(bestLocation) < reservationService.GetOccupancyForLocation(location.Id)) bestLocation = location.Id;
                if (reservationService.GetOccupancyForLocation(worstLocation) > reservationService.GetOccupancyForLocation(location.Id)) worstLocation = location.Id;
            }

            return new Tuple<LocationDTO, LocationDTO>(GetByIdDTO(bestLocation), GetByIdDTO(worstLocation));
        }

        public LocationDTO GetLocation(string country, string city)
        {
            foreach (LocationDTO location in GetAllDTO())
            {
                if (location.Country == country && location.City == city)
                {
                    return location;
                }
            }
            return null;
        }

        public string GetCountryByLocationId(int locationId)
        {
            var location = GetAll().FirstOrDefault(l => l.Id == locationId);
            return location?.Country ?? "Unknown";
        }

        public string GetCityByLocationId(int locationId)
        {
            var location = GetAll().FirstOrDefault(l => l.Id == locationId);
            return location?.City ?? "Unknown";
        }
    }
}

using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Observer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Repository
{
    public class AccommodationRepository : IAccommodationRepository { 
        private const string filePath = "../../../Resources/Data/accommodations.csv";
        private readonly Serializer<Accommodation> _serializer;

        private List<Accommodation> _accommodations;
        public AccommodationRepository()
        {
            _serializer = new Serializer<Accommodation>();
            _accommodations = _serializer.FromCSV(filePath);
    }

        //The method GetAll in service binds locations to accommodations so there i no need for this constructor
        public AccommodationRepository(LocationRepository locationRepository)
        {
            _serializer = new Serializer<Accommodation>();
            _accommodations = _serializer.FromCSV(filePath);

            foreach (Accommodation accommodation in _accommodations)
            {
                foreach (Location location in locationRepository.GetAll())
                {
                    if (accommodation.LocationId == location.Id)
                    {
                        accommodation.Location = location;
                    }
                }
            }

        }

        public Accommodation Save(Accommodation accommodation)
        {
            accommodation.Id = NextId();
            _accommodations = _serializer.FromCSV(filePath);
            _accommodations.Add(accommodation);
            _serializer.ToCSV(filePath, _accommodations);

            return accommodation;
        }

        private int NextId()
        {
            _accommodations = _serializer.FromCSV(filePath);
            if(_accommodations.Count < 1)
            {
                return 0;
            }
            return _accommodations.Max(a => a.Id) + 1;
        }

        public void Delete(Accommodation accommodation)
        {
            _accommodations = _serializer.FromCSV(filePath);
            Accommodation? founded = _accommodations.Find(a => a.Id == accommodation.Id);
            _accommodations.Remove(founded);
            _serializer.ToCSV(filePath, _accommodations);
        }

        public Accommodation Update(Accommodation accommodation)
        {
            _accommodations = _serializer.FromCSV(filePath);
            Accommodation current = _accommodations.Find(a => a.Id == accommodation.Id);
            int index = _accommodations.IndexOf(current);
            _accommodations.Remove(current);
            _accommodations.Insert(index, accommodation);
            _serializer.ToCSV(filePath, _accommodations);
            return accommodation;
        }

        public Accommodation GetById(int id)
        {
            _accommodations = _serializer.FromCSV(filePath);
            return _accommodations.Find(a => a.Id == id);
        }

        public List<Accommodation> GetAll()
        {
            return _accommodations;
        }

    }
}

using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class OwnerAccommodationRatingRepository : IOwnerAccommodationRatingRepository
    {
        private const string filePath = "../../../Resources/Data/ownerAccommodationRatings.csv";
        private Serializer<OwnerAccommodationRating> _serializer;
        private List<OwnerAccommodationRating> ownerRatings;
    
        public OwnerAccommodationRatingRepository()
        {
            _serializer = new();
            ownerRatings = _serializer.FromCSV(filePath);
        }

        public OwnerAccommodationRating Save(OwnerAccommodationRating ownerRating)
        {
            ownerRating.Id = NextId();
            ownerRatings = _serializer.FromCSV(filePath);
            ownerRatings.Add(ownerRating);
            _serializer.ToCSV(filePath, ownerRatings);
            
            return ownerRating;
        }

        public OwnerAccommodationRating Update(OwnerAccommodationRating ownerRating)
        {
            ownerRatings = _serializer.FromCSV(filePath);
            OwnerAccommodationRating current = ownerRatings.Find(a => a.Id == ownerRating.Id);
            int index = ownerRatings.IndexOf(current);
            ownerRatings.Remove(current);
            ownerRatings.Insert(index, ownerRating);
            _serializer.ToCSV(filePath, ownerRatings);
            return ownerRating;
        }

        public int NextId()
        {
            if(ownerRatings.Count() < 1)
            {
                return 0;
            }

            return ownerRatings.Max(x => x.Id) + 1;
        }

        public List<OwnerAccommodationRating> GetAll()
        {
            return _serializer.FromCSV(filePath);
        }

        public OwnerAccommodationRating GetById(int id)
        {
            ownerRatings = _serializer.FromCSV(filePath);
            return ownerRatings.FirstOrDefault(u => u.Id == id);
        }
        
    }
}

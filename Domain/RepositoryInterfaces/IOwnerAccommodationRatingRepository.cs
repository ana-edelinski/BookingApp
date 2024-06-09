using BookingApp.Domain.Model;
using System.Collections.Generic;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IOwnerAccommodationRatingRepository
    {
        public OwnerAccommodationRating Save(OwnerAccommodationRating ownerAccommodationRating);
        public OwnerAccommodationRating Update(OwnerAccommodationRating ownerAccommodationRating);
        public List<OwnerAccommodationRating> GetAll();
        public OwnerAccommodationRating GetById(int id);
    }
}

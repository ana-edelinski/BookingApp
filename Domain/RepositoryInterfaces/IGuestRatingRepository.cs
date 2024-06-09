using BookingApp.Domain.Model;
using System.Collections.Generic;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IGuestRatingRepository
    {
        public GuestRating Save(GuestRating guestRating);
        public GuestRating Update(GuestRating guestRating);
        public List<GuestRating> GetAll();
    }
}

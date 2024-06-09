using BookingApp.Domain.Model;
using System.Collections.Generic;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IAccommodationRepository
    {
        public Accommodation Save(Accommodation accommodation);
        public Accommodation Update(Accommodation accommodation);
        public void Delete(Accommodation accommodation);
        public Accommodation GetById(int id);
        public List<Accommodation> GetAll();

    }
}

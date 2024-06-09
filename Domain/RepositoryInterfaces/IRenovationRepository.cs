using BookingApp.Domain.Models;
using System.Collections.Generic;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IRenovationRepository
    {
        public Renovation Save(Renovation renovation);
        public void Delete(Renovation renovation);
        public List<Renovation> GetAll();
    }
}

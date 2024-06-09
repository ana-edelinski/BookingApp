using BookingApp.Domain.Model;
using System.Collections.Generic;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ILocationRepository
    {
        public Location Save(Location location);
        public Location GetById(int id);
        public List<Location> GetAll();
    }
}

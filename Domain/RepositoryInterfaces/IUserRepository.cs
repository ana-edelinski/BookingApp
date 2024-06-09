using BookingApp.Domain.Model;
using System.Collections.Generic;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IUserRepository
    {
        public void SaveUser(User user);
        public User GetById(int id);
        //
        public User Update(User user);
        public List<User> GetAll();
    }
}

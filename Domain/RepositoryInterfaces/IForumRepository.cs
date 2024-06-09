using BookingApp.Domain.Model;
using BookingApp.Domain.Models;
using System.Collections.Generic;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IForumRepository
    {
        public Forum Save(Forum forum);
        public Forum GetById(int id);
        public List<Forum> GetAll();
        public Forum Update(Forum forum);
    }
}

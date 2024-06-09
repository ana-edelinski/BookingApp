using BookingApp.Domain.Model;
using System.Collections.Generic;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ICommentRepository
    {
        public Comment Save(Comment comment);
        public Comment Update(Comment comment);
        public void Delete(Comment comment);
        public List<Comment> GetByUser(User user);
        public List<Comment> GetAll();
        public Comment GetCommentById(int id);
    }
}

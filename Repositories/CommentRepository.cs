using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Repository
{
    public class CommentRepository : ICommentRepository
    {

        private const string filePath = "../../../Resources/Data/comments.csv";

        private readonly Serializer<Comment> _serializer;

        private List<Comment> _comments;

        public CommentRepository()
        {
            _serializer = new Serializer<Comment>();
            _comments = _serializer.FromCSV(filePath);
        }

        public List<Comment> GetAll()
        {
            return _serializer.FromCSV(filePath);
        }

        public Comment Save(Comment comment)
        {
            comment.Id = NextId();
            _comments = _serializer.FromCSV(filePath);
            _comments.Add(comment);
            _serializer.ToCSV(filePath, _comments);
            return comment;
        }

        public int NextId()
        {
            _comments = _serializer.FromCSV(filePath);
            if (_comments.Count < 1)
            {
                return 0;
            }
            return _comments.Max(c => c.Id) + 1;
        }

        public void Delete(Comment comment)
        {
            _comments = _serializer.FromCSV(filePath);
            Comment found = _comments.Find(c => c.Id == comment.Id);
            _comments.Remove(found);
            _serializer.ToCSV(filePath, _comments);
        }

        public Comment Update(Comment comment)
        {
            _comments = _serializer.FromCSV(filePath);
            Comment current = _comments.Find(c => c.Id == comment.Id);
            int index = _comments.IndexOf(current);
            _comments.Remove(current);
            _comments.Insert(index, comment);       // keep ascending order of ids in file 
            _serializer.ToCSV(filePath, _comments);
            return comment;
        }

        public List<Comment> GetByUser(User user)
        {
            _comments = _serializer.FromCSV(filePath);
            return _comments.FindAll(c => c.User.Id == user.Id);
        }

        public Comment GetCommentById(int id)
        {
            _comments = _serializer.FromCSV(filePath);
            return _comments.FirstOrDefault(c => c.Id == id);
        }
    }
}

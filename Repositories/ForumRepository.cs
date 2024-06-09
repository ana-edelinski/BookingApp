using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.View.OwnerView;
using BookingApp.WPF.View;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Repositories
{
    public class ForumRepository : IForumRepository
    {
        private const string filePath = "../../../Resources/Data/forums.csv";
        private readonly Serializer<Forum> _serializer;
        private  List<Forum> forums;
        public ForumRepository()
        { 
            _serializer = new Serializer<Forum>();
            forums = _serializer.FromCSV(filePath);
        }
        public List<Forum> GetAll()
        {
            return forums;
        }

        public Forum GetById(int id)
        {
            return _serializer.FromCSV(filePath).Find(forum => forum.Id == id);
        }

        private int NextId()
        {
            forums = _serializer.FromCSV(filePath);
            if (forums.Count < 1)
            {
                return 1;
            }
            return forums.Max(a => a.Id) + 1;
        }

        public Forum Save(Forum forum)
        {
            forum.Id = NextId();
            forums = _serializer.FromCSV(filePath);
            forums.Add(forum);
            _serializer.ToCSV(filePath, forums);

            return forum;
        }

        public Forum Update(Forum forum)
        {
            forums = _serializer.FromCSV(filePath);
            Forum current = forums.Find(c => c.Id == forum.Id);
            int index = forums.IndexOf(current);
            forums.Remove(current);
            forums.Insert(index, forum);
            _serializer.ToCSV(filePath, forums);
            return forum;
        }
    }
}

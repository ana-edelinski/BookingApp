using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Observer;
using BookingApp.WPF.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BookingApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private const string filePath = "../../../Resources/Data/users.csv";

        private readonly Serializer<User> _serializer;
        private List<User> _users;



        public UserRepository()
        {
            _serializer = new Serializer<User>();
            _users = _serializer.FromCSV(filePath);

        }

        //remove getbyusername
        public User GetByUsername(string username)
        {
            _users = _serializer.FromCSV(filePath);
            return _users.FirstOrDefault(u => u.Username == username);
        }

        public List<User> GetAll()
        {
            return _users;
        }

        private int GetNewId()
        {
            if (_users.Count == 0)
            {
                return 1;
            }
            return _users[_users.Count - 1].Id + 1;
        }


        public void SaveUser(User user)
        {
            user.Id = GetNewId();
            _users.Add(user);
            _serializer.ToCSV(filePath, _users);
        }
        
        public User GetById(int id)
        {
            //return (User)_users.Select(user => user.Id == id);
            _users = _serializer.FromCSV(filePath);
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public User Update(User user)
        {
            _users = _serializer.FromCSV(filePath);
            User current = _users.Find(c => c.Id == user.Id);
            int index = _users.IndexOf(current);
            _users.Remove(current);
            _users.Insert(index, user);       
            _serializer.ToCSV(filePath, _users);
            return user;
        }

    }
}
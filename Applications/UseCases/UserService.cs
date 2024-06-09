using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model;
using BookingApp.Domain.Model.Enums;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repository;
using BookingApp.WPF.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace BookingApp.Applications.UseCases
{
    public class UserService : IUserRepository
    {
        private readonly IUserRepository _repository;
        private readonly AccommodationReservationService _accommodationReservationService;

        public UserService()
        {
            _repository = Injector.CreateInstance<IUserRepository>();
            _accommodationReservationService = new AccommodationReservationService();
        }

        public User Update(User user)
        {
            return _repository.Update(user);
        }

        public List<User> GetAll()
        {
            return _repository.GetAll();
        }

        public List<UserDTO> GetAllDTO()
        {
            List<UserDTO> users = new();
            foreach(User user in GetAll())
            {
                users.Add(new UserDTO(user));
            }

            return users;
        }
        public User GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void SaveUser(User user)
        {
            _repository.SaveUser(user);
        }

        public User GetByUsername(string username)
        {
           return GetAll().FirstOrDefault(u => u.Username == username);
        }

        public void UpdateUserTitle(int id)
        {
            DateTime oneYearAgo = DateTime.Now.AddYears(-1);
            int numberOfReservations = _accommodationReservationService.GetAll()
                .Where(i => i.GuestId == id && i.StartDate >= oneYearAgo)
                .Count();
            User guest = _repository.GetById(id);

            if (guest.UserType == UserType.GUEST)
            {
                ConsiderPromotionToSuperGuest(guest, numberOfReservations);
            }
            else if (guest.UserType == UserType.SUPERGUEST)
            {
                ConsiderReturningToGuest(guest, numberOfReservations);
            }

            guest.NumberOfReservations = numberOfReservations;
            _repository.Update(guest);
        }

        private void ConsiderPromotionToSuperGuest(User guest, int numberOfReservations)
        {
            if (numberOfReservations >= 10)
            {
                guest.UserType = UserType.SUPERGUEST;
                guest.SuperGuestExpirationDate = DateTime.Now.AddDays(365);
                guest.BonusPoints = 5;
            }
        }

        private void ConsiderReturningToGuest(User guest, int numberOfReservations)
        {
            if (numberOfReservations < 10)
            {
                guest.UserType = UserType.GUEST;
                guest.SuperGuestExpirationDate = null;
                guest.BonusPoints = null;
            }
        }

    }
}

using BookingApp.Domain.Model;
using BookingApp.Domain.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.DTOs
{
    public class UserDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }
        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                if (username != value)
                {
                    username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        private UserType userType;
        public UserType UserType
        {
            get { return userType; }
            set
            {
                if (userType != value)
                {
                    userType = value;
                    OnPropertyChanged(nameof(UserType));
                }
            }
        }

        private int? bonusPoints;
        public int? BonusPoints
        {
            get { return bonusPoints; }
            set
            {
                if (bonusPoints != value)
                {
                    bonusPoints = value;
                    OnPropertyChanged(nameof(BonusPoints));
                }
            }
        }

        private DateTime? superGuestExpirationDate;
        public DateTime? SuperGuestExpirationDate
        {
            get { return superGuestExpirationDate; }
            set
            {
                if (superGuestExpirationDate != value)
                {
                    superGuestExpirationDate = value;
                    OnPropertyChanged(nameof(SuperGuestExpirationDate));
                }
            }
        }

        public User ToUser()
        {
            return new User(Id, Username, Password, UserType);
        }
        public UserDTO(User user)
        {
            Id = user.Id;
            Username = user.Username;
            Password = user.Password;
            UserType = user.UserType;
            BonusPoints = user.BonusPoints;
            SuperGuestExpirationDate = user.SuperGuestExpirationDate;
        }

        public UserDTO()
        {

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

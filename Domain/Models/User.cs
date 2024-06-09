using BookingApp.Applications.UtilityInterfaces;
using BookingApp.Domain.Model.Enums;
using System;
using System.Data;

namespace BookingApp.Domain.Model
{
    public class User : ISerializable
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }
        public int? NumberOfReservations { get; set; }
        public int? BonusPoints { get; set; }
        public DateTime? SuperGuestExpirationDate { get; set; }
        public User() { }

        public User(int id, string username, string password, UserType userType)
        {
            Id = id;
            Username = username;
            Password = password;
            UserType = userType;
        }

        public string[] ToCSV()
        {
            string numberOfReservations = NumberOfReservations.HasValue ? NumberOfReservations.Value.ToString() : "";
            string bonusPoints = BonusPoints.HasValue ? BonusPoints.Value.ToString() : "";
            string superGuestExpirationDate = SuperGuestExpirationDate.HasValue ? SuperGuestExpirationDate.Value.ToString() : "";

            string[] csvValues = { Id.ToString(), Username, Password, UserType.ToString(), numberOfReservations, bonusPoints, superGuestExpirationDate };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Username = values[1];
            Password = values[2];
            UserType = (UserType)Enum.Parse(typeof(UserType), values[3]);
            NumberOfReservations = string.IsNullOrEmpty(values[4]) ? null : (int?)Convert.ToInt32(values[4]);
            BonusPoints = string.IsNullOrEmpty(values[5]) ? null : (int?)Convert.ToInt32(values[5]);
            SuperGuestExpirationDate = string.IsNullOrEmpty(values[6]) ? null : (DateTime?)Convert.ToDateTime(values[6]);
        }
    }
}
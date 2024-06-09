using BookingApp.Applications.UtilityInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class Location : ISerializable
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public Location()
        {
            Country = string.Empty;
            City = string.Empty;
        }
        public Location(int id, string city, string country)
        {
            Id = id;
            City = city;
            Country = country;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            City = values[1];
            Country = values[2];
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), City, Country };
            return csvValues;
        }
    }
}

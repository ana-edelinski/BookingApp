using BookingApp.Applications.UtilityInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public class Renovation : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Renovation()
        {

        }

        public Renovation(int id, int accommodationId, DateTime startDate, DateTime endDate)
        {
            Id = id;
            AccommodationId = accommodationId;
            StartDate = startDate;
            EndDate = endDate;
        }

        public void FromCSV(string[] csvValues)
        {
            Id = int.Parse(csvValues[0]);
            AccommodationId = int.Parse(csvValues[1]);  
            StartDate = DateTime.Parse(csvValues[2]);
            EndDate = DateTime.Parse(csvValues[3]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationId.ToString(),
                StartDate.ToString(),
                EndDate.ToString()
            };
            return csvValues;
        }
    }
}

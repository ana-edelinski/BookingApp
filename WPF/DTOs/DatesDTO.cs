using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.DTOs
{
    public class DatesDTO
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DatesDTO() { }
        public DatesDTO(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}

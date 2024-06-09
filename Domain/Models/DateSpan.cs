using System;

namespace BookingApp.Domain.Model
{
    public class DateSpan
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DateSpan() { }
        public DateSpan(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}

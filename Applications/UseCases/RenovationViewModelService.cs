using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;

namespace BookingApp.Applications.UseCases
{
    public class RenovationViewModelService
    {
        private int estimatedLength;

        public RenovationViewModelService(int estimatedLength)
        {
            this.estimatedLength = estimatedLength;
        }

        public List<DateSpan> FindDateRanges(List<DateTime> dates)
        {
            var dateRanges = new List<DateSpan>();

            for (int i = 0; i < dates.Count - estimatedLength + 1; i++)
            {
                DateTime startDate = dates[i];
                DateTime endDate = dates[i + estimatedLength - 1];

                if (IsValidDateRange(dates, i))
                {
                    dateRanges.Add(new DateSpan { StartDate = startDate, EndDate = endDate });
                }
            }

            return dateRanges;
        }

        private bool IsValidDateRange(List<DateTime> dates, int startIndex)
        {
            for (int i = startIndex + 1; i <= startIndex + estimatedLength - 2; i++)
            {
                if (!IsFollowingDate(dates, i))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsFollowingDate(List<DateTime> dates, int index)
        {
            return dates[index].Subtract(dates[index - 1]).Days == 1;
        }

    }
}

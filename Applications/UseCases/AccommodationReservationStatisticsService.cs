using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;

namespace BookingApp.Applications.UseCases
{
    public class AccommodationReservationStatisticsService
    {
        private readonly AccommodationReservationService accommodationReservationService;

        public AccommodationReservationStatisticsService()
        {
            accommodationReservationService = new();
        }

        public int CountByMonth(int accommodationId, int month, int year, bool isCanceled)
        {
            int count = 0;

            foreach (AccommodationReservation accommodationReservation in accommodationReservationService.GetAll().FindAll(r => r.AccommodationId == accommodationId))
            {
                if (isCanceled)
                {
                    if (((accommodationReservation.StartDate.Month == month && accommodationReservation.StartDate.Year == year) || (accommodationReservation.EndDate.Month == month && accommodationReservation.EndDate.Year == year)) && accommodationReservation.IsCanceled)
                    {
                        count++;
                    }
                }
                else
                {
                    if (((accommodationReservation.StartDate.Month == month && accommodationReservation.StartDate.Year == year) || (accommodationReservation.EndDate.Month == month && accommodationReservation.EndDate.Year == year)) && !accommodationReservation.IsCanceled)
                    {
                        count++;
                    }
                }

            }

            return count;
        }

        public int CountByYear(int accommodationId, int year, bool isCanceled)
        {
            int count = 0;

            foreach (AccommodationReservation accommodationReservation in accommodationReservationService.GetAll().FindAll(r => r.AccommodationId == accommodationId))
            {
                if (isCanceled)
                {
                    if ((accommodationReservation.StartDate.Year == year || accommodationReservation.EndDate.Year == year) && accommodationReservation.IsCanceled)
                    {
                        count++;
                    }
                }
                else
                {
                    if ((accommodationReservation.StartDate.Year == year || accommodationReservation.EndDate.Year == year) && !accommodationReservation.IsCanceled)
                    {
                        count++;
                    }
                }

            }

            return count;
        }

        public int GetOccupancyByMonth(int accommodationId, int month, int year)
        {
            int dayCount = 0;

            foreach (AccommodationReservation accommodationReservation in accommodationReservationService.GetAll().FindAll(r => r.AccommodationId == accommodationId))
            {
                if (!accommodationReservation.IsCanceled)
                {
                    DateTime startDate = accommodationReservation.StartDate;
                    DateTime endDate = accommodationReservation.EndDate;

                    if (startDate.Month == month && startDate.Year == year)
                    {
                        int daysInReservation = Math.Min(DateTime.DaysInMonth(year, month) - startDate.Day + 1, (endDate - startDate).Days + 1);
                        dayCount += daysInReservation;
                    }
                    else if (endDate.Month == month && endDate.Year == year)
                    {
                        int daysInReservation = Math.Min(DateTime.DaysInMonth(year, month), endDate.Day);
                        dayCount += daysInReservation;
                    }
                    else if (startDate < new DateTime(year, month, 1) && endDate >= new DateTime(year, month, DateTime.DaysInMonth(year, month)))
                    {
                        dayCount += DateTime.DaysInMonth(year, month);
                    }

                }
            }

            return 100 * dayCount / DateTime.DaysInMonth(year, month);
        }

        public int GetOccupancyByYear(int accommodationId, int year)
        {
            int dayCount = 0;

            foreach (AccommodationReservation accommodationReservation in accommodationReservationService.GetAll().FindAll(r => r.AccommodationId == accommodationId))
            {
                if (!accommodationReservation.IsCanceled)
                {
                    DateTime startDate = accommodationReservation.StartDate;
                    DateTime endDate = accommodationReservation.EndDate;

                    if (startDate.Year == year && endDate.Year == year)
                    {
                        int daysInReservation = (endDate - startDate).Days + 1;
                        dayCount += daysInReservation;
                    }
                    else if (startDate.Year < year && endDate.Year > year)
                    {
                        dayCount += (new DateTime(year, 12, 31) - new DateTime(year, 1, 1)).Days + 1;
                    }
                    else if (startDate.Year == year && endDate.Year > year)
                    {
                        dayCount += (new DateTime(year, 12, 31) - startDate).Days + 1;
                    }
                    else if (startDate.Year < year && endDate.Year == year)
                    {
                        dayCount += (endDate - new DateTime(year, 1, 1)).Days + 1;
                    }
                }
            }

            return 100 * dayCount / 365;
        }

        public List<int> GetAllYears(int accommodationId)
        {
            List<int> years = new();

            foreach (AccommodationReservation accommodationReservation in accommodationReservationService.GetAll().FindAll(r => r.AccommodationId == accommodationId))
            {
                if (!years.Contains(accommodationReservation.StartDate.Year)) years.Add(accommodationReservation.StartDate.Year);
                if (!years.Contains(accommodationReservation.EndDate.Year)) years.Add(accommodationReservation.EndDate.Year);
            }

            return years;
        }


    }
}

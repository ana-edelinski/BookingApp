using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repository;
using BookingApp.WPF.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Applications.UseCases
{
    public class AccommodationReservationService : IAccommodationReservationRepository
    {
        private readonly IAccommodationReservationRepository _repository;
        private readonly RenovationService renovationService;
        private readonly AccommodationService accommodationService;
        public AccommodationReservationService()
        {
            _repository = Injector.CreateInstance<IAccommodationReservationRepository>();
            renovationService = new();
            accommodationService = new();
        }
        public void Delete(AccommodationReservation accommodationReservation)
        {
            _repository.Delete(accommodationReservation);
        }

        public List<AccommodationReservation> GetAll()
        {
            return _repository.GetAll();
        }

        public List<AccommodationReservationDTO> GetAllDTO()
        {
            List<AccommodationReservationDTO> accommodationReservationDTOs = new();

            foreach(AccommodationReservation accommodationReservation in GetAll())
            {
                accommodationReservationDTOs.Add(new AccommodationReservationDTO(accommodationReservation));
            }

            return accommodationReservationDTOs;
        }

        public AccommodationReservation GetById(int id)
        {
            return _repository.GetById(id);
        }

        public AccommodationReservationDTO GetByIdDTO(int id)
        {
            return _repository.GetByIdDTO(id);
        }

        public AccommodationReservation Save(AccommodationReservation accommodationReservation)
        {
            return _repository.Save(accommodationReservation);
        }

        public AccommodationReservation Update(AccommodationReservation accommodationReservation)
        {
            return _repository.Update(accommodationReservation);
        }

        private List<DateTime> AddTakenDates(int accommodationId, DateTime startDate, DateTime endDate)
        {
            List<DateTime> reservedDates = renovationService.GetTakenDates(accommodationId);
            foreach (AccommodationReservation reservation in _repository.GetAll())
            {
                if (reservation.AccommodationId == accommodationId && reservation.StartDate >= startDate && reservation.StartDate <= endDate)
                {
                    for (DateTime date = reservation.StartDate; date <= reservation.EndDate; date = date.AddDays(1))
                    {
                        reservedDates.Add(date);
                    }
                }
            }

            return reservedDates;
        }

        public List<DateTime> GetAvailableDates(int accommodationId, DateTime startDate, DateTime endDate, int? duration = 0)
        {
            List<DateTime> reservedDates = new List<DateTime>();
            foreach (AccommodationReservation reservation in _repository.GetAll())
            {
                if (reservation.AccommodationId == accommodationId && reservation.StartDate >= startDate && reservation.StartDate <= endDate)
                {
                    for (DateTime date = reservation.StartDate; date <= reservation.EndDate; date = date.AddDays(1))
                    {
                        reservedDates.Add(date);
                    }
                }
            }

            List<DateTime> availableDates = new List<DateTime>();
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (!reservedDates.Contains(date))
                {
                    availableDates.Add(date);
                }
            }

            if (duration > 0)
            {
                List<DateTime> filteredDates = new List<DateTime>();

                foreach (DateTime date in availableDates)
                {
                    bool isAvailable = true;

                    for (int i = 0; i < duration; i++)
                    {
                        if (!availableDates.Contains(date.AddDays(i)))
                        {
                            isAvailable = false;
                            break;
                        }
                    }

                    if (isAvailable)
                    {
                        filteredDates.Add(date);
                    }
                }

                return filteredDates;
            }
            else
            {
                return availableDates;
            }
        }


        public bool CanMoveReservation(int accommodationId, DateTime startDate, DateTime endDate)
        {
            List<DateTime> reservedDates = renovationService.GetTakenDates(accommodationId);

            foreach(AccommodationReservation reservation in _repository.GetAll().FindAll(reservation => reservation.AccommodationId == accommodationId)) 
            {
                

                if (reservation.StartDate >= endDate || reservation.EndDate >= startDate) return false;


                if (reservation.StartDate <= startDate && reservation.EndDate >= endDate) return false;


                if (reservation.StartDate >= startDate) return false;
                
            }

            return true;
        }


        public int GetNumberOfReservationsForLocation(int locationId)//for registration/removal recommendation
        {
            int numberOfReservations = 0;
            
            foreach(AccommodationReservation accommodationReservation in GetAll().FindAll(aR => accommodationService.GetAll().Any(a => a.LocationId == locationId && aR.AccommodationId == a.Id)))
            {
                numberOfReservations++;
            }

            return numberOfReservations;
        }

        public int GetOccupancyForLocation(int locationId)//for registration/removal recommendation
        {
            int occupancy = 0;

            foreach (AccommodationReservation accommodationReservation in GetAll().FindAll(aR => accommodationService.GetAll().Any(a => a.LocationId == locationId && aR.AccommodationId == a.Id)))
            {
                occupancy += accommodationReservation.Capacity / accommodationService.GetAll().Find(a => a.Id == accommodationReservation.AccommodationId).Capacity;
            }

            return occupancy;
        }

        public int GetAccommodationIdByReservationId(int reservationId)
        {
            var reservation = _repository.GetById(reservationId);
            return reservation.AccommodationId;
        }

        public string GetOwnerNameForAccommodationReservation(int accommodationReservationId)
        {
            var reservation = _repository.GetById(accommodationReservationId);
            if (reservation != null)
            {
                var userService = new UserService();
                var ownerId = accommodationService.GetOwnerIdForAccommodation(reservation.AccommodationId);
                var owner = userService.GetById(ownerId);
                return owner?.Username ?? "Unknown";
            }

            return "Unknown";
        }

        public string GetAccommodationNameForAccommodationReservation(int accommodationReservationId)
        {
            var reservation = _repository.GetById(accommodationReservationId);
            if (reservation != null)
            {
                var accommodationService = new AccommodationService();
                var accommodation = accommodationService.GetById(reservation.AccommodationId);
                return accommodation.Name;
            }

            return "Unknown";
        }

        public List<AccommodationReservation> GetByAccommodationId(int accommodationId)
        {
            return _repository.GetByAccommodationId(accommodationId);
        }

        public List<AccommodationReservation> GetReservationsByGuestId(int guestId)
        {
            List<AccommodationReservation> reservations = new List<AccommodationReservation>();
            foreach (AccommodationReservation reservation in _repository.GetAll())
            {
                if (reservation.GuestId == guestId)
                {
                    reservations.Add(reservation);
                }
            }
            return reservations;
        }

        public bool HasGuestVisitedLocation(int guestId, int locationId)
        {
            List<AccommodationReservation> reservations = GetReservationsByGuestId(guestId);

            foreach (AccommodationReservation reservation in reservations)
            {
                Accommodation accommodation = accommodationService.GetById(reservation.AccommodationId);
                if (accommodation.LocationId == locationId)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
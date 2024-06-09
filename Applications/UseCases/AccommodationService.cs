using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repository;
using BookingApp.WPF.DTOs;
using System;
using System.Collections.Generic;

namespace BookingApp.Applications.UseCases
{
    public class AccommodationService : IAccommodationRepository
    {
        private readonly IAccommodationRepository accommodationRepository;
        private readonly LocationService locationService;
        private readonly IAccommodationReservationRepository accommodationReservationRepository;
        public AccommodationService() 
        {
            accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
            locationService = new();
            accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
        }

        public void Delete(Accommodation accommodation)
        {
            accommodationRepository.Delete(accommodation);
        }

        public List<Accommodation> GetAll()
        {
            List<Accommodation> accommodations = new();
            foreach (Accommodation accommodation in accommodationRepository.GetAll())
            {
                foreach (Location location in locationService.GetAll())
                {
                    if (accommodation.LocationId == location.Id)
                    {
                        accommodation.Location = location;
                        accommodations.Add(accommodation);
                    }
                }
            }
            return accommodations;
        }

        public Accommodation GetById(int id)
        {
            return accommodationRepository.GetById(id);
        }

        public Accommodation Save(Accommodation accommodation)
        {
            return accommodationRepository.Save(accommodation);
        }

        public Accommodation Update(Accommodation accommodation)
        {
            return accommodationRepository.Update(accommodation);
        }

        public List<AccommodationDTO> GetAllDTO()
        {
            List<AccommodationDTO> accommodations = new();

            foreach(Accommodation accommodation in GetAll())
            {
                accommodations.Add(new AccommodationDTO(accommodation));
            }

            return accommodations;
        }

        public List<AccommodationDTO> GetAllForOwner(int ownerId)
        {
            return GetAllDTO().FindAll(accommodation => accommodation.OwnerId == ownerId);
        }

        public List<Accommodation> GetAvailable(int? numberOfGuests, int? numberOfDays)
        {
            List<Accommodation> availableAccommodations = new List<Accommodation>();

            foreach (Accommodation accommodation in accommodationRepository.GetAll())
            {
                bool hasEnoughGuestCapacity = accommodation.Capacity >= numberOfGuests;
                bool meetsMinReservationDays = accommodation.MinReservationDays <= numberOfDays;

                if (hasEnoughGuestCapacity && meetsMinReservationDays)
                {
                    availableAccommodations.Add(accommodation);
                }
            }

            return availableAccommodations;
        }

        public List<Accommodation> GetAvailableAccommodations(DateTime startDate, DateTime endDate, int numberOfGuests, int numberOfDays)
        {
            List<Accommodation> availableAccommodations = new List<Accommodation>();

            foreach (Accommodation accommodation in accommodationRepository.GetAll())
            {
                bool isAvailable = IsAccommodationAvailable(accommodation, startDate, endDate);
                bool hasEnoughGuestCapacity = accommodation.Capacity >= numberOfGuests;
                bool meetsMinReservationDays = accommodation.MinReservationDays <= numberOfDays;

                if (isAvailable && hasEnoughGuestCapacity && meetsMinReservationDays)
                {
                    availableAccommodations.Add(accommodation);
                }
            }

            return availableAccommodations;
        }

        private bool IsAccommodationAvailable(Accommodation accommodation, DateTime startDate, DateTime endDate)
        {
            List<AccommodationReservation> reservations = accommodationReservationRepository.GetByAccommodationId(accommodation.Id);    //treba service ali stackoverflow...

            foreach (AccommodationReservation reservation in reservations)
            {
                if (startDate <= reservation.EndDate && endDate >= reservation.StartDate)
                {
                    return false;
                }
            }

            return true;
        }

        public int GetCancelDaysPriorReservationForAccommodation(int accommodationId)
        {
            Accommodation accommodation = accommodationRepository.GetById(accommodationId);
            return accommodation.CancelDaysPriorReservation;
        }

        public int GetOwnerIdForAccommodation(int accommodationId)
        {
            Accommodation accommodation = accommodationRepository.GetById(accommodationId);
            return accommodation.OwnerId;
        }


    }
}

using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model;
using BookingApp.Domain.Model.Enums;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Applications.UseCases
{
    public class AccommodationReservationMoveRequestService : IAccommodationReservationMoveRequestRepository
    {
        private readonly IAccommodationReservationMoveRequestRepository _repository;
        private readonly AccommodationReservationService accommodationReservationService;

        public AccommodationReservationMoveRequestService()
        {
            _repository = Injector.CreateInstance<IAccommodationReservationMoveRequestRepository>();
            accommodationReservationService = new();
        }
        public void Delete(AccommodationReservationMoveRequest accommodation)
        {
            _repository.Delete(accommodation);
        }

        public List<AccommodationReservationMoveRequest> GetAll()
        {
            return _repository.GetAll();   
        }

        public List<AccommodationReservationMoveRequestDTO> GetAllDTO()
        {
            List<AccommodationReservationMoveRequestDTO> accommodationReservationMoveRequestDTO = new();

            foreach(AccommodationReservationMoveRequest accommodationReservationMoveRequest in GetAll())
            {
                accommodationReservationMoveRequestDTO.Add(new AccommodationReservationMoveRequestDTO(accommodationReservationMoveRequest));
            }

            return accommodationReservationMoveRequestDTO;
        }

        public AccommodationReservationMoveRequest GetById(int id)
        {
            return _repository.GetById(id);
        }

        public AccommodationReservationMoveRequest Save(AccommodationReservationMoveRequest accommodationReservationMoveRequest)
        {
            return _repository.Save(accommodationReservationMoveRequest);
        }

        public AccommodationReservationMoveRequest Update(AccommodationReservationMoveRequest accommodationReservationMoveRequest)
        {
            return _repository.Update(accommodationReservationMoveRequest);
        }

        public int CountByMonth(int accommodationId, int month, int year)
        {
            int count = 0;

            foreach (AccommodationReservationMoveRequest accommodationReservation in GetAll().FindAll(r => accommodationReservationService.GetAll().Any(ar=> ar.AccommodationId == accommodationId && ar.Id == r.ReservationId)))
            {
                if (((accommodationReservation.StartDate.Month == month && accommodationReservation.StartDate.Year == year) || (accommodationReservation.EndDate.Month == month && accommodationReservation.EndDate.Year == year)) && accommodationReservation.Status == (ReservationMoveRequestStatus)1)
                {
                    count++;
                }
            }

            return count;
        }

        public int CountByYear(int accommodationId, int year)
        {
            int count = 0;

            foreach (AccommodationReservationMoveRequest accommodationReservation in GetAll().FindAll(r => accommodationReservationService.GetAll().Any(ar => ar.AccommodationId == accommodationId && ar.Id == r.ReservationId)))
            {
                if ((accommodationReservation.StartDate.Year == year || accommodationReservation.EndDate.Year == year) && accommodationReservation.Status == (ReservationMoveRequestStatus)1)
                {
                    count++;
                }
            }

            return count;
        }
    }
}

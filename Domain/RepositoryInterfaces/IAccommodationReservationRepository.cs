using BookingApp.Domain.Model;
using BookingApp.WPF.DTOs;
using System.Collections.Generic;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IAccommodationReservationRepository
    {
        public AccommodationReservation Save(AccommodationReservation accommodationReservation);
        public AccommodationReservation Update(AccommodationReservation accommodationReservation);
        public void Delete(AccommodationReservation accommodationReservation);
        public AccommodationReservation GetById(int id);
        public AccommodationReservationDTO GetByIdDTO(int id);  
        public List<AccommodationReservation> GetAll();
        public List<AccommodationReservation> GetByAccommodationId(int accommodationId);

    }
}

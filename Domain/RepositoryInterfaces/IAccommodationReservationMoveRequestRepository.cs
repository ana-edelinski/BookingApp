using BookingApp.Domain.Model;
using BookingApp.Repository;
using System.Collections.Generic;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IAccommodationReservationMoveRequestRepository
    {
        public AccommodationReservationMoveRequest Save(AccommodationReservationMoveRequest accommodationReservationMoveRequest);
        public AccommodationReservationMoveRequest Update(AccommodationReservationMoveRequest accommodationReservationMoveRequest);
        public void Delete(AccommodationReservationMoveRequest accommodation);
        public AccommodationReservationMoveRequest GetById(int id);
        public List<AccommodationReservationMoveRequest> GetAll();
    }
}

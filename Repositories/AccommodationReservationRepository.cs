using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.DTOs;
using BookingApp.Observer;
using BookingApp.View;
using BookingApp.View.OwnerView;
using BookingApp.WPF.View.OwnerView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{

    public class AccommodationReservationRepository : IAccommodationReservationRepository
    {
        private const string filePath = "../../../Resources/Data/reservedAccommodation.csv";
        private readonly Serializer<AccommodationReservation> _serializer;

        private List<AccommodationReservation> accommodationReservations;
        private readonly GuestRatingRepository guestRatingRepository;
        private readonly GuestRating guestRating;

        public AccommodationReservationRepository()
        {
            _serializer = new Serializer<AccommodationReservation>();
            accommodationReservations = _serializer.FromCSV(filePath);
            guestRatingRepository = new GuestRatingRepository();
            guestRating = new();
        }

        public AccommodationReservation Save(AccommodationReservation accommodationReservation)
        {
            accommodationReservation.Id = NextId();
            accommodationReservations = _serializer.FromCSV(filePath);
            accommodationReservations.Add(accommodationReservation);
            _serializer.ToCSV(filePath, accommodationReservations);

            return accommodationReservation;
        }

        public int NextId()
        {
            accommodationReservations = _serializer.FromCSV(filePath);
            if (accommodationReservations.Count < 1)
            {
                return 1;
            }

            return accommodationReservations.Max(rA => rA.Id) + 1;
        }

        /*
        public void Delete(AccommodationReservation accommodationResevation)
        {
            accommodationReservations = _serializer.FromCSV(filePath);
            AccommodationReservation? found = accommodationReservations.Find(a => a.Id == accommodationResevation.Id);
            accommodationReservations.Remove(found);
            _serializer.ToCSV(filePath, accommodationReservations);
        }
        */

        public void Delete(AccommodationReservation accommodationResevation)
        {
            var reservations = _serializer.FromCSV(filePath);
            var existingReservation = reservations.FirstOrDefault(r => r.Id == accommodationResevation.Id);

            if (existingReservation != null)
            {
                reservations.Remove(existingReservation);

                int index = 1;
                foreach (var r in reservations)
                {
                    r.Id = index++;
                }
                _serializer.ToCSV(filePath, reservations);
                accommodationReservations = reservations;
            }
        }
        public AccommodationReservation Update(AccommodationReservation accommodationReservation)
        {
            accommodationReservations = _serializer.FromCSV(filePath);
            AccommodationReservation current = accommodationReservations.Find(a => a.Id == accommodationReservation.Id);
            int index = accommodationReservations.IndexOf(current);
            accommodationReservations.Remove(current);
            accommodationReservations.Insert(index, accommodationReservation);
            _serializer.ToCSV(filePath, accommodationReservations);
            return accommodationReservation;
        }
        public List<AccommodationReservation> GetAll()
        {
            return accommodationReservations;
        }

        public AccommodationReservation GetById(int id)
        {
            accommodationReservations = _serializer.FromCSV(filePath);
            return accommodationReservations.FirstOrDefault(u => u.Id == id);
        }

        public AccommodationReservationDTO GetByIdDTO(int id)
        {
            var reservation = accommodationReservations.FirstOrDefault(r => r.Id == id);

            if (reservation != null)
            {
                return new AccommodationReservationDTO
                {
                    Id = reservation.Id,
                    GuestId = reservation.GuestId,
                    AccommodationId = reservation.AccommodationId,
                    StartDate = reservation.StartDate,
                    EndDate = reservation.EndDate,
                    NumberDays = reservation.NumberDays,
                    IsAvailable = reservation.IsAvailable,
                    Capacity = reservation.Capacity,
                    IsCanceled = reservation.IsCanceled
                };
            }

            return null;
        }

        public List<AccommodationReservation> GetByAccommodationId(int accommodationId)
        {
            return accommodationReservations.Where(r => r.AccommodationId == accommodationId).ToList();
        }

    }
}

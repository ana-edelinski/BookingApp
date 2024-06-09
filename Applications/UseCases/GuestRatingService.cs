using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repository;
using BookingApp.WPF.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Applications.UseCases
{
    public class GuestRatingService : IGuestRatingRepository
    {
        private readonly IGuestRatingRepository _repository;

        public GuestRatingService()
        {
            _repository = Injector.CreateInstance<IGuestRatingRepository>();
        }
        public List<GuestRating> GetAll()
        {
            return _repository.GetAll();
        }

        public List<GuestRatingDTO> GetAllDTO()
        {
            List<GuestRatingDTO> guestRatingDTOs = new();

            foreach(GuestRating guestRating in GetAll())
            {
                guestRatingDTOs.Add(new GuestRatingDTO(guestRating));
            }

            return guestRatingDTOs;
        }

        public GuestRating Save(GuestRating guestRating)
        {
            return _repository.Save(guestRating);
        }

        public GuestRating Update(GuestRating guestRating)
        {
            return _repository.Update(guestRating);
        }

        public List<GuestRating> GetAllRated()
        {
            List<GuestRating> rated = new();

            foreach (GuestRating guestRating in GetAll())
            {
                if (guestRating.IsRated) rated.Add(guestRating);
            }

            return rated;
        }

        public List<UserDTO> GetUnratedGuests()
        {
            List<UserDTO> unratedGuests = new();
            UserRepository userRepository = new();

            foreach (User user in userRepository.GetAll())
            {
                if (IsGuestUnrated(user)) unratedGuests.Add(new UserDTO( user));

            }

            return unratedGuests;
        }

        private bool IsGuestUnrated(User user)
        {
            AccommodationReservationRepository accommodationReservationRepository = new();

            return GetAll().Any(guestRating => !guestRating.IsRated &&
                                                accommodationReservationRepository.GetAll().Any(accommodationReservation =>
                                                accommodationReservation.IsLessThan5Days() &&
                                                accommodationReservation.GuestId == user.Id &&
                                                accommodationReservation.Id == guestRating.AccommodationReservationId));
        }

    }
}

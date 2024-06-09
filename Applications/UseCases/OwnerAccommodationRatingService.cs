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
    public class OwnerAccommodationRatingService : IOwnerAccommodationRatingRepository
    {
        private readonly IOwnerAccommodationRatingRepository _repository;
        public OwnerAccommodationRatingService()
        {
            _repository = Injector.CreateInstance<IOwnerAccommodationRatingRepository>();

        }
        public List<OwnerAccommodationRating> GetAll()
        {
            return _repository.GetAll();
        }

        public OwnerAccommodationRating GetById(int id)
        {
            return _repository.GetById(id);
        }

        public OwnerAccommodationRating Save(OwnerAccommodationRating ownerAccommodationRating)
        {
            return _repository.Save(ownerAccommodationRating);
        }

        public OwnerAccommodationRating Update(OwnerAccommodationRating ownerAccommodationRating)
        {
            return _repository.Update(ownerAccommodationRating);
        }

        public List<OwnerAccommodationRating> GetAllForOwner(int ownerId)
        {

            AccommodationReservationRepository accommodationReservationRepository = new(); 
            AccommodationRepository accommodationRepository = new();

            return GetAll().FindAll(ownerAccommodationRating => ownerAccommodationRating.IsRated &&
                                       accommodationReservationRepository.GetAll().Any(accommodationReservation =>
                                       accommodationReservation.Id == ownerAccommodationRating.AccommodationReservationId &&
                                       accommodationRepository.GetAll().Any(accommodation =>
                                       accommodation.OwnerId == ownerId &&
                                       accommodation.Id == accommodationReservation.AccommodationId)));

        }

        public List<OwnerAccommodationRating> GetAllReviews(int ownerId)
        {
            List<OwnerAccommodationRating> reviews = new();
            GuestRatingService guestRatingService = new();

            foreach (OwnerAccommodationRating ownerAccommodationRating in GetAllForOwner(ownerId))
            {
                foreach (GuestRating guestRating in guestRatingService.GetAllRated())
                {
                    if (ownerAccommodationRating.AccommodationReservationId == guestRating.AccommodationReservationId)
                    {
                        reviews.Add(ownerAccommodationRating);
                    }
                }
            }

            return reviews;
        }

        public List<OwnerAccommodationRatingDTO> GetAllReviewsDTO(int ownerId)
        {
            List<OwnerAccommodationRatingDTO> ownerAccommodationRatingDTOs = new();
            foreach(OwnerAccommodationRating ownerAccommodationRating in GetAllReviews(ownerId))
            {
                ownerAccommodationRatingDTOs.Add(new OwnerAccommodationRatingDTO(ownerAccommodationRating));
            }

            return ownerAccommodationRatingDTOs;
        }

        public bool IsSuperOwner(int ownerId)
        {

            return GetAllForOwner(ownerId).Count >= 50 && CalculateTotalAverage(ownerId) >= 4.5;
        }

        private double CalculateTotalAverage(int ownerId)
        {
            double scoreTotal = 0;
            foreach (OwnerAccommodationRating ownerAccommodationRating in GetAllForOwner(ownerId))
            {
                scoreTotal += ownerAccommodationRating.AverageRating;
            }

            return scoreTotal / GetAllForOwner(ownerId).Count;
        }

        public List<AccommodationReservationDTO> GetUnratedAccommodations()
        {
            List<AccommodationReservationDTO> unratedAccommodations = new();
            AccommodationReservationService accommodationReservationService = new(); 

            foreach (AccommodationReservationDTO accommodationReservation in accommodationReservationService.GetAllDTO())
            {
                if (IsAccommodationUnrated(accommodationReservation)) 
                    unratedAccommodations.Add(accommodationReservation);
            }

            return unratedAccommodations;
        }

        private bool IsAccommodationUnrated(AccommodationReservationDTO accommodationReservation)
        {
            AccommodationReservationRepository accommodationReservationRepository = new();
            var fiveDaysAgo = DateTime.Now.AddDays(-5);


            if (accommodationReservation.EndDate < fiveDaysAgo) 
            {
                return false; 
            }


            var accommodationRating = GetAll().FirstOrDefault(accommodationRating =>
                accommodationRating.AccommodationReservationId == accommodationReservation.Id &&
                accommodationRating.IsRated);

            return accommodationRating == null;
        }


    }
}


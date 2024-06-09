using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.Repository;
using System;
using System.Collections.Generic;

namespace BookingApp.Applications.Utilities
{
    public class Injector
    {
        private static Dictionary<Type, object> _implementations = new()
        {
            { typeof(IAccommodationReservationMoveRequestRepository), new AccommodationReservationMoveRequestRepository() },
            { typeof(IAccommodationReservationRepository), new AccommodationReservationRepository() },
            { typeof(IAccommodationRepository), new AccommodationRepository() },
            { typeof(ICommentRepository), new CommentRepository() },
            { typeof(IGuestRatingRepository), new GuestRatingRepository() },
            { typeof(IImageRepository), new ImageRepository() },
            { typeof(ILocationRepository), new LocationRepository() },
            { typeof(IOwnerAccommodationRatingRepository), new OwnerAccommodationRatingRepository() },
            { typeof(IUserRepository), new UserRepository() },
            { typeof(IRenovationRepository), new RenovationRepository() },
            { typeof(IForumRepository), new ForumRepository() },
            { typeof(IRenovationRecommendationRepository), new RenovationRecommendationRepository() },
            //Add your tour and other implementations here...
        };

        public static T CreateInstance<T>()
        {
            Type type = typeof(T);

            if (_implementations.ContainsKey(type))
            {
                return (T)_implementations[type];
            }

            throw new ArgumentException($"No implementation found for type {type}");
        }
    }
}

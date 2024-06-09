using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BookingApp.Repository
{
    public class GuestRatingRepository : IGuestRatingRepository
    {
        private const string filePath = "../../../Resources/Data/guestRatings.csv";
        private readonly Serializer<GuestRating> _serializer;

        private List<GuestRating> guestRatings;
        public GuestRatingRepository()
        {
            _serializer = new();
            guestRatings = _serializer.FromCSV(filePath);
        }
        public GuestRating Save(GuestRating guestRating)
        {
            guestRatings = _serializer.FromCSV(filePath);
            guestRating.Id = NextId();
            guestRatings.Add(guestRating);
            _serializer.ToCSV(filePath, guestRatings);

            return guestRating;
        }

        public int NextId()
        {
            guestRatings = _serializer.FromCSV(filePath);

            if (guestRatings.Count < 1)
            {
                return 0;
            }
            return guestRatings.Max(guestRating => guestRating.Id) + 1;
        }

        public GuestRating Update(GuestRating guestRating)
        {
            guestRatings = _serializer.FromCSV(filePath);
            GuestRating? current = guestRatings.Find(gR => gR.Id == guestRating.Id);
            int index = guestRatings.IndexOf(current);
            guestRatings.Remove(current);
            guestRatings.Insert(index, guestRating);
            _serializer.ToCSV(filePath, guestRatings);

            return guestRating;
        }

        public List<GuestRating> GetAll()
        {
            return _serializer.FromCSV(filePath);
        }

    }
}
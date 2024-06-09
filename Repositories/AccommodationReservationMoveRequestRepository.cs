using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.View.OwnerView;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Repository
{
    public class AccommodationReservationMoveRequestRepository : IAccommodationReservationMoveRequestRepository
    {
        private const string filePath = "../../../Resources/Data/accommodationReservationMoveRequests.csv";
        private readonly Serializer<AccommodationReservationMoveRequest> _serializer;
        private  List<AccommodationReservationMoveRequest> _moveRequests;

        public AccommodationReservationMoveRequestRepository()
        {
            _serializer = new();
            _moveRequests = _serializer.FromCSV(filePath);
        }

        public AccommodationReservationMoveRequest Save(AccommodationReservationMoveRequest accommodationResevationMoveRequest)
        {
            accommodationResevationMoveRequest.Id = NextId();
            _moveRequests = _serializer.FromCSV(filePath);
            _moveRequests.Add(accommodationResevationMoveRequest);
            _serializer.ToCSV(filePath, _moveRequests);

            return accommodationResevationMoveRequest;
        }

        public int NextId()
        {
            _moveRequests = _serializer.FromCSV(filePath);
            if (_moveRequests.Count < 1)
            {
                return 0;
            }
            return _moveRequests.Max(a => a.Id) + 1;
        }

        public void Delete(AccommodationReservationMoveRequest accommodationResevationMoveRequest)
        {
            _moveRequests = _serializer.FromCSV(filePath);
            AccommodationReservationMoveRequest? founded = _moveRequests.Find(a => a.Id == accommodationResevationMoveRequest.Id);
            _moveRequests.Remove(founded);
            _serializer.ToCSV(filePath, _moveRequests);
        }

        public AccommodationReservationMoveRequest Update(AccommodationReservationMoveRequest accommodationResevationMoveRequest)
        {
            _moveRequests = _serializer.FromCSV(filePath);
            AccommodationReservationMoveRequest current = _moveRequests.Find(a => a.Id == accommodationResevationMoveRequest.Id);
            int index = _moveRequests.IndexOf(current);
            _moveRequests.Remove(current);
            _moveRequests.Insert(index, accommodationResevationMoveRequest);
            _serializer.ToCSV(filePath, _moveRequests);
            return accommodationResevationMoveRequest;
        }

        public AccommodationReservationMoveRequest GetById(int id)
        {
            _moveRequests = _serializer.FromCSV(filePath);
            return _moveRequests.Find(a => a.Id == id);
        }

        public List<AccommodationReservationMoveRequest> GetAll()
        {
            return _moveRequests;
        }
    }
}

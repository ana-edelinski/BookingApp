using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.WPF.DTOs;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BookingApp.WPF.ViewModel.OwnerViewModel
{
    public class ReservationsViewModel : NotifyPropertyChangedImplemented
    {
        private readonly AccommodationReservationService accommodationReservationService = new();
        private readonly AccommodationService accommodationService = new();
        private readonly ImageService imageService = new();
        private readonly UserService userService = new();

        private IEnumerable<dynamic> reservations;
        public IEnumerable<dynamic> Reservations
        {
            get { return reservations; }
            set
            {
                if (reservations != value)
                {
                    reservations = value;
                    OnPropertyChanged(nameof(Reservations));
                }
            }
        }

        public void GetReservationsDataGrid(UserDTO user)
        {
            

            var dataObjects = from accommodation in accommodationService.GetAll()
                              join image in imageService.BindImageToAccommodation() on accommodation.Id equals image.AccommodationId
                              join reservation in accommodationReservationService.GetAllDTO() on accommodation.Id equals reservation.AccommodationId
                              join guest in userService.GetAll() on reservation.GuestId equals guest.Id
                              where accommodation.OwnerId == user.Id && reservation.IsCanceled == false
                              select new
                              {
                                  Accommodation = accommodation,
                                  Image = image,
                                  Reservation = reservation,
                                  Guest = guest
                              };

            Reservations = dataObjects;
        }

        public ReservationsViewModel(UserDTO user)
        {
            GetReservationsDataGrid(user);
        }
    }
}

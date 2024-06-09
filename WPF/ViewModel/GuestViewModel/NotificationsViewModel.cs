using BookingApp.Applications.UseCases;
using BookingApp.Domain.Model.Enums;
using BookingApp.WPF.DTOs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookingApp.WPF.ViewModel.GuestViewModel
{
    public class NotificationsViewModel : INotifyPropertyChanged
    {
        private readonly UserDTO user;
        private readonly AccommodationReservationMoveRequestService _accommodationReservationMoveRequestService = new();


        private ObservableCollection<string> _notifications;
        public ObservableCollection<string> Notifications
        {
            get => _notifications;
            set
            {
                if (_notifications != value)
                {
                    _notifications = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _imageSource;
        public string ImageSource
        {
            get => _imageSource;
            set
            {
                if (_imageSource != value)
                {
                    _imageSource = value;
                    OnPropertyChanged(nameof(ImageSource));
                }
            }
        }


        public NotificationsViewModel(UserDTO user)
        {
            this.user = user;
            Notifications = new ObservableCollection<string>();
            CheckRescheduleRequestsStatus();
        }

        public void CheckRescheduleRequestsStatus()
        {
            var requests = _accommodationReservationMoveRequestService.GetAll();

            Notifications.Clear();

            foreach (var request in requests)
            {
                if (request.Status == ReservationMoveRequestStatus.APPROVED || request.Status == ReservationMoveRequestStatus.REJECTED)
                {
                    string period = $"{request.StartDate:MM/dd/yyyy} - {request.EndDate:MM/dd/yyyy}";
                    Notifications.Add($"The status of reschedule request for period\n{period} has been changed to\n{request.Status}.");
                }
            }

            if (Notifications.Count == 0)
            {
                Notifications.Add("You're all caught up");
                ImageSource = "/Resources/GuestIcons/notifications.png";
            }
            else
            {
                ImageSource = "/Resources/GuestIcons/notifications-new.png";
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

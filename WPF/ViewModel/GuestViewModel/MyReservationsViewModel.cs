using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model;
using BookingApp.WPF.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Navigation;
using Image = BookingApp.Domain.Model.Image;

namespace BookingApp.WPF.ViewModel.GuestViewModel
{
    public class MyReservationsViewModel : INotifyPropertyChanged
    {
        private readonly AccommodationReservationService _accommodationReservationService = new();
        private readonly ImageService _imageService = new();
        private readonly AccommodationService _accommodationService = new();
        private readonly UserDTO user;
        public ObservableCollection<AccommodationReservationDTO> PresentableReservations { get; set; }
        public AccommodationReservationDTO SelectedReservation { get; set; }
        public AccommodationDTO SelectedAccommodation { get; set; }

        public RelayCommand CancelReservationCommand { get; set; }
        public MyReservationsViewModel(UserDTO user, NavigationService navigationService)
        {
            this.user = user;
            SelectedReservation = new AccommodationReservationDTO();

            LoadReservations();

            CancelReservationCommand = new RelayCommand(Execute_CancelReservationCommand, CanExecute_Command);
        }


        private bool CanExecute_Command(object obj)
        {
            return true;
        }

        public void Execute_CancelReservationCommand(object obj)
        {
            var selectedReservation = obj as AccommodationReservationDTO;

            var messageBoxResult = MessageBox.Show($"Are you sure you want to cancel reservation for date: {selectedReservation.StartDate:d} - {selectedReservation.EndDate:d}", "Cancel Reservation Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (CancellationPeriodPassed(selectedReservation)) return;
                selectedReservation.IsCanceled = true;
                _accommodationReservationService.Update(selectedReservation.ToReservation());
                MessageBox.Show("Reservation canceled successfully.");
                PresentableReservations.Remove(selectedReservation);
                OnPropertyChanged(nameof(PresentableReservations));
            }
        }

        public bool CancellationPeriodPassed(AccommodationReservationDTO reservation)
        {
            int accommodationId = _accommodationReservationService.GetAccommodationIdByReservationId(reservation.Id);
            int cancelDaysPriorReservation = _accommodationService.GetCancelDaysPriorReservationForAccommodation(accommodationId);

            TimeSpan timeLeft = reservation.StartDate - DateTime.Now;
            if (timeLeft.TotalHours <= 24)
            {
                ShowCancelWarning();
                return true;
            }
            else if (timeLeft.TotalDays <= cancelDaysPriorReservation)
            {
                ShowCancelPeriodWarning(reservation);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ShowCancelWarning()
        {
            MessageBox.Show("Cannot cancel reservation. Less than 24 hours left before start date.", "Cancel reservation warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void ShowCancelPeriodWarning(AccommodationReservationDTO reservation)
        {
            int accommodationId = _accommodationReservationService.GetAccommodationIdByReservationId(reservation.Id);
            int cancelDaysPriorReservation = _accommodationService.GetCancelDaysPriorReservationForAccommodation(accommodationId);
            MessageBox.Show($"Cannot cancel reservation. Less than {cancelDaysPriorReservation} left before start date.", "Cancel reservation warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void LoadReservations()
        {            
            var reservations = _accommodationReservationService.GetAll()
                .Where(r => !r.IsCanceled)
                .Select(r => new AccommodationReservationDTO
                {
                    Id = r.Id,
                    GuestId = r.GuestId,
                    AccommodationId = r.AccommodationId,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    NumberDays = r.NumberDays,
                    IsAvailable = r.IsAvailable,
                    Capacity = r.Capacity,
                    ImagePath = GetImageForAccommodation(r.AccommodationId),
                    AccommodationName = _accommodationReservationService.GetAccommodationNameForAccommodationReservation(r.Id)
                });

            PresentableReservations = new ObservableCollection<AccommodationReservationDTO>(reservations);
        }

        private string GetImageForAccommodation(int accommodationId)
        {
            var image = BindImageToAccommodation().FirstOrDefault(i => i.AccommodationId == accommodationId);
            return image?.Path; 
        }

        private List<Image> BindImageToAccommodation()
        {
            List<Image> images = new();
            foreach (Accommodation accommodation in _accommodationService.GetAll())
            {
                images.Add(_imageService.GetOneForAccommodation(accommodation.Id));
            }
            return images;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

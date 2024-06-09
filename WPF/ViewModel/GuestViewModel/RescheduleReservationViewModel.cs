using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model.Enums;
using BookingApp.WPF.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace BookingApp.WPF.ViewModel.GuestViewModel
{
    public class RescheduleReservationViewModel : INotifyPropertyChanged
    {
        private readonly AccommodationReservationMoveRequestService _accommodationReservationMoveRequestService;
        private readonly AccommodationService _accommodationService = new();
        private readonly ImageService _imageService = new();


        public AccommodationReservationDTO SelectedReservation { get; set; }
        public AccommodationReservationMoveRequestDTO SelectedReservationMoveRequest { get; set; }
        public AccommodationDTO SelectedAccommodation { get; set; }

        public CommentDTO Comment { get; set; }
        public RelayCommand SendRequestCommand { get; set; }

        private DateTime _newStartDate;
        public DateTime NewStartDate
        {
            get { return _newStartDate; }
            set
            {
                if (_newStartDate != value)
                {
                    _newStartDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _newEndDate;
        public DateTime NewEndDate
        {
            get { return _newEndDate; }
            set
            {
                if (_newEndDate != value)
                {
                    _newEndDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public RescheduleReservationViewModel(MyReservationsViewModel myReservationsViewModel)
        {
            SelectedReservation = myReservationsViewModel.SelectedReservation;
            _newStartDate = SelectedReservation.StartDate;
            _newEndDate = SelectedReservation.EndDate;

            _accommodationReservationMoveRequestService = new AccommodationReservationMoveRequestService();

            SelectedReservationMoveRequest = new AccommodationReservationMoveRequestDTO();
            Comment = new CommentDTO();

            SendRequestCommand = new RelayCommand(Execute_SendRequestCommand, CanExecute_Command);
        }

        private bool CanExecute_Command(object obj)
        {
            return true;
        }

        public void Execute_SendRequestCommand(object obj)
        {
            var messageBoxResult = MessageBox.Show($"Are you sure you want to reserve another date: {NewStartDate:d} - {NewEndDate:d}?", "Reschedule Request Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (DatesValid(SelectedReservation) && NumberOfDaysValid(SelectedReservation))
                {
                    var accommodationReservationMoveRequest = new AccommodationReservationMoveRequestDTO(SelectedReservationMoveRequest.Id, SelectedReservation.Id, NewStartDate, NewEndDate, (ReservationMoveRequestStatus)0, Comment.Id);
                    _accommodationReservationMoveRequestService.Save(accommodationReservationMoveRequest.ToRequest());
                    MessageBox.Show("Reschedule request sent successfully.", "Reschedule Request", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private bool DatesValid(AccommodationReservationDTO selectedReservation)
        {
            if (NewStartDate < DateTime.Today)
            {
                ShowStartDateError();
                return false;
            }
            else if (NewStartDate == selectedReservation.StartDate && NewEndDate == selectedReservation.EndDate)
            {
                ShowSameDateError();
                return false;
            }
            else if (NewEndDate < NewStartDate)
            {
                ShowEndDateError();
                return false;
            }
            return true;
        }

        private void ShowStartDateError()
        {
            MessageBox.Show("The new start date cannot be any date that has passed.", "Reschedule Request Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void ShowSameDateError()
        {
            MessageBox.Show("Selected dates are the same as the existing reservation dates. Please select different dates.", "Reschedule Request Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void ShowEndDateError()
        {
            MessageBox.Show("The new end date cannot be earlier than the start date.", "Reschedule Request Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool NumberOfDaysValid(AccommodationReservationDTO selectedReservation)
        {
            int numberOfDays = (NewEndDate - NewStartDate).Days;
            int expectedNumberOfDays = (selectedReservation.EndDate - selectedReservation.StartDate).Days;

            if (numberOfDays != expectedNumberOfDays)
            {
                MessageBox.Show("The number of days must be the same as in the existing reservation.", "Reschedule Request Error", MessageBoxButton.OK);
                return false;
            }
            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}

using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model;
using BookingApp.Domain.Model.Enums;
using BookingApp.WPF.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace BookingApp.WPF.ViewModel.GuestViewModel
{
    public class AvailabilityViewModel : INotifyPropertyChanged

    {
        private readonly UserDTO user;
        private readonly AccommodationService _accommodationService = new();
        private readonly AccommodationReservationService _accommodationReservationService = new();
        private readonly ImageService _imageService = new();
        private readonly UserService _userService = new();

        public RelayCommand ReserveCommand { get; set; }

        public AccommodationDTO SelectedAvailableAccommodation { get; set; }
        public DatesDTO SelectedDate { get; set; }

        private ObservableCollection<DatesDTO> _dateIntervals;
        public ObservableCollection<DatesDTO> DateIntervals
        {
            get { return _dateIntervals; }
            set
            {
                _dateIntervals = value;
                OnPropertyChanged("DateIntervals");
            }
        }

        private int? _numberOfDays;
        public int? NumberOfDays
        {
            get { return _numberOfDays; }
            set
            {
                _numberOfDays = value;
                LoadAvailableDates();
                OnPropertyChanged("NumberOfDays");
            }
        }

        private int? _numberOfGuests;
        public int? NumberOfGuests
        {
            get { return _numberOfGuests; }
            set
            {
                _numberOfGuests = value;
                OnPropertyChanged("NumberOfGuests");
            }
        }

        private string _selectedAccommodationImagePath;
        public string SelectedAccommodationImagePath
        {
            get { return _selectedAccommodationImagePath; }
            set
            {
                _selectedAccommodationImagePath = value;
                OnPropertyChanged(nameof(SelectedAccommodationImagePath));
            }
        }

        public AvailabilityViewModel(AccommodationDTO selectedAvailableAccommodation, AccommodationService accommodationService, AccommodationReservationService accommodationReservationService, int? numberOfDays, int? numberOfGuests, UserDTO user)
        {
            this.user = user;
            SelectedAvailableAccommodation = selectedAvailableAccommodation;
            DateIntervals = new ObservableCollection<DatesDTO>();
            NumberOfDays = numberOfDays;
            NumberOfGuests = numberOfGuests;

            ReserveCommand = new RelayCommand(Execute_ReserveCommand, CanExecute_Command);

            DisplayImage();
            LoadAvailableDates();
        }

        private void LoadAvailableDates()
        {
            var suggestedDateRange = GetSuggestedDateRange();
            DateTime startDate = suggestedDateRange.startDate;
            DateTime endDate = suggestedDateRange.endDate;

            List<DateTime> availableDates = _accommodationReservationService.GetAvailableDates(SelectedAvailableAccommodation.Id, startDate, endDate, NumberOfDays);

            DateIntervals.Clear();

            ObservableCollection<DateTime> selectedDates = new ObservableCollection<DateTime>(availableDates);

            List<DatesDTO> dateRanges = FindDateRanges(selectedDates);

            foreach (var dateRange in dateRanges)
            {
                if (IsValidDateRange(selectedDates, selectedDates.IndexOf(dateRange.StartDate)))
                {
                    AddDateInterval(dateRange.StartDate, dateRange.EndDate);
                }
            }
        }

        private List<DatesDTO> FindDateRanges(ObservableCollection<DateTime> dates)
        {
            var dateRanges = new List<DatesDTO>();

            if (NumberOfDays.HasValue)
            {
                int numberOfDays = NumberOfDays.Value;

                for (int i = 0; i < dates.Count - numberOfDays + 1; i++)
                {
                    DateTime startDate = dates[i];
                    DateTime endDate = dates[i].AddDays(numberOfDays - 1);
                    dateRanges.Add(new DatesDTO { StartDate = startDate, EndDate = endDate });
                }
            }
            return dateRanges;
        }


        private bool IsValidDateRange(ObservableCollection<DateTime> dates, int startIndex)
        {
            for (int i = startIndex + 1; i < startIndex + NumberOfDays; i++)
            {
                if (!IsDateFollowsPreviousDate(dates, i))
                {
                    return false;
                }
            }

            return true;
        }
        private bool IsDateFollowsPreviousDate(ObservableCollection<DateTime> dates, int index)
        {
            return dates[index].Subtract(dates[index - 1]).Days == 1;
        }

        private void AddDateInterval(DateTime startDate, DateTime endDate)
        {
            if (NumberOfDays.HasValue)
            {
                int numberOfDays = NumberOfDays.Value;

                if (endDate - startDate == TimeSpan.FromDays(numberOfDays - 1))
                {
                    DateIntervals.Add(new DatesDTO
                    {
                        StartDate = startDate,
                        EndDate = endDate
                    });
                }
            }
        }


        private (DateTime startDate, DateTime endDate) GetSuggestedDateRange()
        {
            int daysToAdd = 25;
            var newStartDate = DateTime.Today.AddDays(-daysToAdd);

            if (newStartDate < DateTime.Today)
                newStartDate = DateTime.Today;

            var newEndDate = DateTime.Today.AddDays(daysToAdd);
            return (newStartDate.Date, newEndDate.Date);
        }

        private void DisplayImage()
        {
            var image = BindImageToAccommodation().FirstOrDefault(i => i.AccommodationId == SelectedAvailableAccommodation.Id);
            SelectedAccommodationImagePath = image?.Path;
        }

        private List<ImageDTO> BindImageToAccommodation()
        {
            List<ImageDTO> images = new();
            foreach (AccommodationDTO accommodation in _accommodationService.GetAllDTO())
            {
                images.Add(_imageService.GetOneForAccommodationDTO(accommodation.Id));
            }
            return images;
        }

        private bool CanExecute_Command(object obj)
        {
            return true;
        }

        public void Execute_ReserveCommand(object obj)
        {
            if (SelectedDate != null)
            {
                DateTime? startDate = SelectedDate.StartDate;
                DateTime? endDate = SelectedDate.EndDate;

                if (startDate != null && endDate != null && NumberOfDays != null && NumberOfGuests != null)
                {
                    var messageBoxResult = MessageBox.Show($"Are you sure you want to reserve the date: {startDate:d} - {endDate:d}", "Reserve Accomodation Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        var accommodation = _accommodationService.GetById(SelectedAvailableAccommodation.Id);
                        var reservation = new AccommodationReservation(user.Id, SelectedAvailableAccommodation.Id, startDate.Value, endDate.Value, NumberOfDays.Value, NumberOfGuests.Value);
                        _accommodationReservationService.Save(reservation);

                        MessageBox.Show("Reservation created successfully.");

                        DateTime oneYearAgo = DateTime.Now.AddYears(-1);
                        int numberOfReservations = _accommodationReservationService.GetAll()
                            .Where(i => i.GuestId == user.Id && i.StartDate >= oneYearAgo)
                            .Count();
                        if (numberOfReservations == 10)
                        {
                            MessageBox.Show("Congratulations! You've become super-guest!");
                        }
                        if (user.UserType == UserType.SUPERGUEST)
                        {
                            UpdateUserBonusPoints(user.Id);
                        }
                    }
                }
                return;
            }
        }


        public void UpdateUserBonusPoints(int userId)
        {
            var user = _userService.GetById(userId);
            user.NumberOfReservations += 1;
            user.BonusPoints -= 1;
            _userService.Update(user);
            string message = $"You have {user.BonusPoints} bonus points left.";
            MessageBox.Show(message, "Information about bonus points", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
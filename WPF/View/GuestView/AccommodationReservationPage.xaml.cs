using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BookingApp.Domain.Model;
using BookingApp.Applications.UseCases;
using BookingApp.WPF.DTOs;
using System.Diagnostics;
using Image = BookingApp.Domain.Model.Image;
using BookingApp.Domain.Model.Enums;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for AccommodationReservationPage.xaml
    /// </summary>
    public partial class AccommodationReservationPage : Page
    {
        private readonly AccommodationReservationService _accommodationReservationService;
        private readonly AccommodationService _accommodationService;
        private readonly UserService _userService;
        private readonly ImageService _imageService;

        public User LoggedInUser { get; set; }


        private AccommodationReservation _reservation;

        public AccommodationReservation Reservation
        {
            get { return _reservation; }
            set
            {
                if (_reservation != value)
                {
                    _reservation = value;
                    OnPropertyChanged(nameof(Reservation));
                }
            }
        }


        private Accommodation _selectedAccommodation;
        public Accommodation SelectedAccommodation
        {
            get { return _selectedAccommodation; }
            set
            {
                if (_selectedAccommodation != value)
                {
                    _selectedAccommodation = value;
                    OnPropertyChanged(nameof(SelectedAccommodation));
                }
            }
        }


        private ObservableCollection<DateSpan> _dateIntervals;
        public ObservableCollection<DateSpan> DateIntervals
        {
            get { return _dateIntervals; }
            set
            {
                _dateIntervals = value;
                OnPropertyChanged("DateIntervals");
            }
        }
        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }
        private DateTime _endDate;
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }

        private int _numberOfDays;
        public int NumberOfDays
        {
            get => _numberOfDays;
            set
            {
                if (value != _numberOfDays)
                {
                    _numberOfDays = value;
                    OnPropertyChanged();
                }
            }
        }

        private readonly UserDTO user;

        private int _maxGuests;
        public int MaxGuests
        {
            get => _maxGuests;
            set
            {
                if (value != _maxGuests)
                {
                    _maxGuests = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _ownerId;
        public int OwnerId
        {
            get => _ownerId;
            set
            {
                if (value != _ownerId)
                {
                    _ownerId = value;
                    OnPropertyChanged();
                }
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

        public AccommodationReservationPage(Accommodation selectedAccommodation, UserDTO user, AccommodationService accommodationService, AccommodationReservationService accommodationReservationService)
        {
            InitializeComponent();
            DataContext = this;

            SelectedAccommodation = selectedAccommodation;
            this.user = user;

            Reservation = new AccommodationReservation();
            Reservation.AccommodationId = selectedAccommodation.Id;


            _accommodationService = accommodationService;
            _accommodationReservationService = new AccommodationReservationService();

            DateIntervals = new ObservableCollection<DateSpan>();
            _userService = new UserService();
            _imageService = new ImageService();

            DisplayImage();

        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            DateIntervals.Clear();

            if (!ValidateDates()) return;
            if (!ValidateNumberOfDays()) return;
            if (!ValidateNumberOfGuests()) return;

            ObservableCollection<DateTime> allFreeDates = GetAllFreeDates();

            AddDateRanges(FindDateRanges(allFreeDates));

            if (DateIntervals.Count == 0)
            {
                var messageBoxResult = MessageBox.Show($"No available dates! Would you like to choose alternative?", "Suggested Accomodation Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (messageBoxResult == MessageBoxResult.Yes)
                {

                    int DaysToAdd = 25;

                    if (DaysToAdd > 0)
                    {
                        DateTime newStartDate = StartDate.AddDays(-DaysToAdd);
                        if (newStartDate >= DateTime.Today)
                        {
                            StartDate = newStartDate.Date;
                        }
                        else
                        {
                            StartDate = DateTime.Today.Date;
                        }
                    }

                    EndDate = EndDate.Date.AddDays(DaysToAdd);

                    allFreeDates = GetAllFreeDates();

                    AddDateRanges(FindDateRanges(allFreeDates));
                }

                return;

            }
        }

        private ObservableCollection<DateTime> GetAllFreeDates()
        {
            int accommodationId = Reservation.AccommodationId;
            int duration = NumberOfDays;
            DateTime startDate = StartDate;
            DateTime endDate = EndDate;

            return new ObservableCollection<DateTime>(_accommodationReservationService.GetAvailableDates(accommodationId, startDate, endDate, duration));
        }

        private void AddDateRanges(List<DateSpan> dateRanges)
        {
            foreach (var dateRange in dateRanges)
            {
                DateIntervals.Add(dateRange);
            }
        }

        private bool ValidateDates()
        {
            StartDate = startDatePicker.SelectedDate.GetValueOrDefault();
            EndDate = endDatePicker.SelectedDate.GetValueOrDefault();

            if (StartDate == default || EndDate == default)
            {
                ShowNoDateTimeWarning();
                return false;
            }

            if (StartDate.Date < DateTime.Today || EndDate.Date < DateTime.Today || StartDate.Date > EndDate.Date)
            {
                ShowInvalidDateTimeWarning();
                return false;
            }

            return true;
        }

        private bool ValidateNumberOfDays()
        {
            if (!int.TryParse(numDaysTextBox.Text, out int numberOfDays) || numberOfDays <= 0)
            {
                ShowInvalidNumberWarning();
                return false;
            }

            if (numberOfDays <= 0)
            {
                ShowNoNumberWarning();
                return false;
            }

            if (numberOfDays < SelectedAccommodation.MinReservationDays)
            {
                ShowMinimumReservationDaysWarning();
                return false;
            }

            NumberOfDays = numberOfDays;
            return true;
        }

        private bool ValidateNumberOfGuests()
        {
            if (!int.TryParse(maxGuestsTextBox.Text, out int numberGuests))
            {
                ShowInvalidInputWarning();
                return false;
            }

            if (numberGuests <= 0)
            {
                ShowNoInputWarning();
                return false;
            }

            if (numberGuests > SelectedAccommodation.Capacity)
            {
                ShowMaxGuestsWarning();
                return false;
            }

            MaxGuests = numberGuests;
            return true;
        }

        private List<DateSpan> FindDateRanges(ObservableCollection<DateTime> dates)
        {
            var dateRanges = new List<DateSpan>();

            for (int i = 0; i < dates.Count - NumberOfDays + 1; i++)
            {
                DateTime startDate = dates[i];
                DateTime endDate = dates[i + NumberOfDays - 1];

                if (IsValidDateRange(dates, i))
                {
                    dateRanges.Add(new DateSpan { StartDate = startDate, EndDate = endDate });
                }
            }

            return dateRanges;
        }

        private bool IsValidDateRange(ObservableCollection<DateTime> dates, int startIndex)
        {
            for (int i = startIndex + 1; i <= startIndex + NumberOfDays - 2; i++)
            {
                if (!IsFollowingDate(dates, i))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsFollowingDate(ObservableCollection<DateTime> dates, int index)
        {
            return dates[index].Subtract(dates[index - 1]).Days == 1;
        }

        
        private void MakeReservation(object sender, RoutedEventArgs e)
        {
            
            var selectedItem = dateSpansDataGrid.SelectedItem as DateSpan;

            if (selectedItem != null)
            {
                var messageBoxResult = MessageBox.Show($"Are you sure you want to confirm this reservation: {selectedItem.StartDate:d} - {selectedItem.EndDate:d}", "Reserve Accomodation Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    var reservation = new AccommodationReservation(user.Id, SelectedAccommodation.Id, selectedItem.StartDate, selectedItem.EndDate, NumberOfDays, MaxGuests);

                    _accommodationReservationService.Save(reservation);

                    MessageBox.Show("Reservation created successfully!");

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
                return;
            }
            
        }


        private void DisplayImage()
        {
            var image = BindImageToAccommodation().FirstOrDefault(i => i.AccommodationId == SelectedAccommodation.Id);
            SelectedAccommodationImagePath = image?.Path;
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

        public void UpdateUserBonusPoints(int userId)
        {
            var user = _userService.GetById(userId);
            user.NumberOfReservations += 1;
            user.BonusPoints -= 1;
            _userService.Update(user);
            string message = $"You have {user.BonusPoints} bonus points left.";
            MessageBox.Show(message, "Information about bonus points", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void ShowNoDateTimeWarning()
        {
            MessageBox.Show("Please enter date!", "Date warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void ShowInvalidDateTimeWarning()
        {
            MessageBox.Show("Choose valid date!", "Date", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void ShowNoNumberWarning()
        {
            MessageBox.Show("Please enter number of days!", "Number of days warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void ShowInvalidNumberWarning()
        {
            MessageBox.Show("Enter valid number!", "Number of days warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void ShowMinimumReservationDaysWarning()
        {
            MessageBox.Show($"Number of days must be at least {SelectedAccommodation.MinReservationDays}", "Number of days warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void ShowNoInputWarning()
        {
            MessageBox.Show("Enter number of guests!", "Number of guests warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void ShowInvalidInputWarning()
        {
            MessageBox.Show("Enter valid number.", "Number of guests warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void ShowMaxGuestsWarning()
        {
            MessageBox.Show($"Maximum number of guests is {SelectedAccommodation.Capacity}", "Number of guests warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

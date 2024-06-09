using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.WPF.DTOs;
using BookingApp.WPF.View.GuestView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace BookingApp.WPF.ViewModel.GuestViewModel
{
    public class AnywhereAnytimeViewModel : INotifyPropertyChanged
    {
        private readonly ImageService _imageService = new();
        private readonly AccommodationService _accommodationService = new();
        private readonly AccommodationReservationService _accommodationReservationService = new();
        private readonly UserDTO user;

        public ObservableCollection<AccommodationDTO> PresentableAccommodations { get; set; }
        public AccommodationDTO SelectedAccommodation { get; set; }

        public RelayCommand PageSwitchCommand { get; set; }
        public RelayCommand SearchCommand { get; set; }
        public RelayCommand CancelSearchCommand { get; set; }

        private NavigationService Navigate { get; set; }

        private bool _isDemoRunning = false;
        private DispatcherTimer _demoTimer;
        private int _demoStep = 0;

        public RelayCommand DemoCommand { get; set; }
        public RelayCommand StopDemoCommand { get; set; }

        private int? _numberOfGuests;
        public int? NumberOfGuests
        {
            get { return _numberOfGuests; }
            set { _numberOfGuests = value; OnPropertyChanged(); }
        }

        private int? _numberOfDays;
        public int? NumberOfDays
        {
            get { return _numberOfDays; }
            set { _numberOfDays = value; OnPropertyChanged(); }
        }


        private DateTime _selectedStartDate;
        public DateTime SelectedStartDate
        {
            get { return _selectedStartDate; }
            set { _selectedStartDate = value; OnPropertyChanged(); }
        }

        private DateTime _selectedEndDate;
        public DateTime SelectedEndDate
        {
            get { return _selectedEndDate; }
            set { _selectedEndDate = value; OnPropertyChanged(); }
        }

        private bool _isSearchExecuted;
        public bool IsSearchExecuted
        {
            get { return _isSearchExecuted; }
            set { _isSearchExecuted = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AnywhereAnytimeViewModel(UserDTO user, NavigationService navigationService)
        {
            this.user = user;
            Navigate = navigationService;
            LoadAccommodations();

            SelectedStartDate = DateTime.Today;
            SelectedEndDate = DateTime.Today;

            PageSwitchCommand = new RelayCommand(Execute_PageSwitchCommand, CanExecute_PageSwitchCommand);
            SearchCommand = new RelayCommand(Execute_SearchCommand, CanExecute_Command);
            CancelSearchCommand = new RelayCommand(Execute_CancelSearchCommand, CanExecute_Command);

            DemoCommand = new RelayCommand(StartDemo_Click);
            StopDemoCommand = new RelayCommand(StopDemo_Click);
            _demoTimer = new DispatcherTimer();
            _demoTimer.Interval = TimeSpan.FromSeconds(2);
            _demoTimer.Tick += DemoTimer_Tick;
        }


        private async void StartDemo_Click(object sender)
        {
            _isDemoRunning = true;
            _demoStep = 0;
            //MessageBox.Show("Starting Demo");
            _demoTimer.Start();
        }

        private void StopDemo_Click(object sender)
        {
            StopDemo();
        }
        private void StopDemo()
        {
            _isDemoRunning = false;
            _demoTimer.Stop();
            //MessageTextBlock.Text = "Demo stopped.";
        }
        private void DemoTimer_Tick(object sender, EventArgs e)
        {
            if (!_isDemoRunning) return;

            switch (_demoStep)
            {
                case 0:
                    GuestsNumberFill();
                    break;
                case 1:
                    DaysNumberFill();
                    break;
                case 2:
                    DatesFill();
                    break;
                case 3:
                    Search();
                    break;
                case 4:
                    CancelSearch();
                    break;
            }

            _demoStep = (_demoStep + 1) % 10;
        }

        private void GuestsNumberFill()
        {
            NumberOfGuests = 2;

        }

        private void DaysNumberFill()
        {
            NumberOfDays = 2;
        }

        private async Task DatesFill()
        {
            SelectedStartDate = DateTime.Now.AddDays(2);
            SelectedEndDate = DateTime.Now.AddDays(8);
        }

        private void Search()
        {
            Execute_SearchCommand(null);
        }

        private void CancelSearch()
        {
            Execute_CancelSearchCommand(null);
        }


        private void LoadAccommodations()
        {
            var joinedData = from accommodation in _accommodationService.GetAll()
                             join image in BindImageToAccommodation() on accommodation.Id equals image.AccommodationId
                             select new AccommodationDTO
                             {
                                 Id = accommodation.Id,
                                 Name = accommodation.Name,
                                 Location = accommodation.Location,
                                 Type = accommodation.Type,
                                 Capacity = accommodation.Capacity,
                                 MinReservationDays = accommodation.MinReservationDays,
                                 CancelDaysPriorReservation = accommodation.CancelDaysPriorReservation,
                                 ImagePath = GetImageForAccommodation(accommodation.Id)
                             };

            PresentableAccommodations = new ObservableCollection<AccommodationDTO>(joinedData);
        }

        private string GetImageForAccommodation(int accommodationId)
        {
            var image = BindImageToAccommodation().FirstOrDefault(i => i.AccommodationId == accommodationId);
            return image?.Path;
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

        private bool CanExecute_PageSwitchCommand(object obj)
        {
            return IsSearchExecuted;
        }

        private void Execute_PageSwitchCommand(object parameter)
        {
            if (parameter is AccommodationDTO selectedAccommodation)
            {
                SelectedAccommodation = selectedAccommodation;
                Navigate.Navigate(new Availability(selectedAccommodation, _accommodationService, _accommodationReservationService, NumberOfDays, NumberOfGuests, user));
            }
        }
        private void Execute_SearchCommand(object parameter)
        {
            if (SelectedStartDate != null && SelectedEndDate != null && NumberOfGuests.HasValue && NumberOfDays.HasValue)
            {
                DateTime startDate = SelectedStartDate;
                DateTime endDate = SelectedEndDate;
                int numberOfGuests = NumberOfGuests.Value;
                int numberOfDays = NumberOfDays.Value;

                var availableAccommodations = _accommodationService.GetAvailableAccommodations(startDate, endDate, numberOfGuests, numberOfDays);
                var accommodationDTOs = availableAccommodations.Select(accommodation => new AccommodationDTO
                {
                    Id = accommodation.Id,
                    Name = accommodation.Name,
                    Location = accommodation.Location,
                    Type = accommodation.Type,
                    Capacity = accommodation.Capacity,
                    MinReservationDays = accommodation.MinReservationDays,
                    CancelDaysPriorReservation = accommodation.CancelDaysPriorReservation,
                    ImagePath = GetImageForAccommodation(accommodation.Id)
                }).ToList();

                PresentableAccommodations.Clear();
                foreach (var dto in accommodationDTOs)
                {
                    PresentableAccommodations.Add(dto);
                }
            }
            else
            {
                var availableAccommodations = _accommodationService.GetAvailable(NumberOfGuests ?? 0, NumberOfDays ?? 0);
                var accommodationDTOs = availableAccommodations.Select(accommodation => new AccommodationDTO
                {
                    Id = accommodation.Id,
                    Name = accommodation.Name,
                    Location = accommodation.Location,
                    Type = accommodation.Type,
                    Capacity = accommodation.Capacity,
                    MinReservationDays = accommodation.MinReservationDays,
                    CancelDaysPriorReservation = accommodation.CancelDaysPriorReservation,
                    ImagePath = GetImageForAccommodation(accommodation.Id)
                }).ToList();

                PresentableAccommodations.Clear();
                foreach (var dto in accommodationDTOs)
                {
                    PresentableAccommodations.Add(dto);
                }
            }
            IsSearchExecuted = true;
        }


        private void Execute_CancelSearchCommand(object parameter)
        {
            LoadAccommodations();
            OnPropertyChanged(nameof(PresentableAccommodations));
            SelectedStartDate = DateTime.Today;
            SelectedEndDate = DateTime.Today;
            NumberOfGuests = null;
            NumberOfDays = null;
            IsSearchExecuted = false;
        }
    }
}

using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.Domain.Models;
using BookingApp.WPF.DTOs;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace BookingApp.WPF.ViewModel.GuestViewModel
{
    public class RenovationRecommendationViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private readonly RenovationRecommendationService _renovationRecommendationService;
        private readonly AccommodationReservationService _accommodationReservationService;
        public AccommodationReservationDTO Reservation { get; set; }

        public RelayCommand RecommendCommand { get; set; }

        private string _information;
        public string Information
        {
            get => _information;
            set
            {
                if (value != _information)
                {
                    _information = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _renovationLevel;
        public int RenovationLevel
        {
            get => _renovationLevel;
            set
            {
                if (value != _renovationLevel)
                {
                    _renovationLevel = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _radioButton1IsChecked;
        public bool RadioButton1IsChecked
        {
            get => _radioButton1IsChecked;
            set
            {
                if (value != _radioButton1IsChecked)
                {
                    _radioButton1IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _radioButton2IsChecked;
        public bool RadioButton2IsChecked
        {
            get => _radioButton2IsChecked;
            set
            {
                if (value != _radioButton2IsChecked)
                {
                    _radioButton2IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _radioButton3IsChecked;
        public bool RadioButton3IsChecked
        {
            get => _radioButton3IsChecked;
            set
            {
                if (value != _radioButton3IsChecked)
                {
                    _radioButton3IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _radioButton4IsChecked;
        public bool RadioButton4IsChecked
        {
            get => _radioButton4IsChecked;
            set
            {
                if (value != _radioButton4IsChecked)
                {
                    _radioButton4IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _radioButton5IsChecked;
        public bool RadioButton5IsChecked
        {
            get => _radioButton5IsChecked;
            set
            {
                if (value != _radioButton5IsChecked)
                {
                    _radioButton5IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private OwnerAccommodationRatingDTO _selectedUnratedAccommodation;
        public OwnerAccommodationRatingDTO SelectedUnratedAccommodation
        {
            get => _selectedUnratedAccommodation;
            set
            {
                if (value != _selectedUnratedAccommodation)
                {
                    _selectedUnratedAccommodation = value;
                    OnPropertyChanged(nameof(SelectedUnratedAccommodation));
                    OnPropertyChanged(nameof(AccommodationName));
                }
            }
        }
        public string AccommodationName
        {
            get => SelectedUnratedAccommodation?.AccommodationName;
        }

        private bool _isDemoRunning = false;
        private DispatcherTimer _demoTimer;
        private int _demoStep = 0;

        public RelayCommand DemoCommand { get; set; }
        public RelayCommand StopDemoCommand { get; set; }

        public RenovationRecommendationViewModel(RateRecommendViewModel rateRecommendViewModel)
        {
            SelectedUnratedAccommodation = rateRecommendViewModel.SelectedUnratedAccommodation;
            _renovationRecommendationService = new RenovationRecommendationService();
            _accommodationReservationService = new AccommodationReservationService();

            Reservation = _accommodationReservationService.GetByIdDTO(SelectedUnratedAccommodation.AccommodationReservationId);

            RecommendCommand = new RelayCommand(Execute_SendRecommendationCommand, CanExecute_Command);

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
        }
        private void DemoTimer_Tick(object sender, EventArgs e)
        {
            if (!_isDemoRunning) return;

            switch (_demoStep)
            {
                case 0:
                    RenovationLevelFill();
                    break;
                case 1:
                    StateOfAccommodationFill();
                    break;

            }

            _demoStep = (_demoStep + 1) % 10;
        }

        private void RenovationLevelFill()
        {
            RadioButton1IsChecked = true;

        }

        private void StateOfAccommodationFill()
        {
            Information = "10/10";
        }


        private void SetValuesForRenovationLevel()
        {
            if (RadioButton1IsChecked == true)
            {
                RenovationLevel = 1;
            }
            else if (RadioButton2IsChecked == true)
            {
                RenovationLevel = 2;
            }
            else if (RadioButton3IsChecked == true)
            {
                RenovationLevel = 3;
            }
            else if (RadioButton4IsChecked == true)
            {
                RenovationLevel = 4;
            }
            else if (RadioButton5IsChecked == true)
            {
                RenovationLevel = 5;
            }
        }

        private bool CanExecute_Command(object obj)
        {
            return true;
        }

        private void Execute_SendRecommendationCommand(object obj)
        {
            if (IsValid)
            {
                var messageBoxResult = MessageBox.Show($"Would you like to send your recommendation?", "Send Recommendation Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    SetValuesForRenovationLevel();
                    RenovationRecommendation renovationRecommendation = new RenovationRecommendation(Reservation.AccommodationId, Information, RenovationLevel, Reservation.GuestId, DateTime.Now);
                    _renovationRecommendationService.Save(renovationRecommendation);
                    MessageBox.Show("Recommendation for renovation sent successfully.");
                }
            }
        }

        public string Error => null;
        public string this[string columnName]
        {
            get
            {
                string error = null;

                if (columnName == "Information" && string.IsNullOrEmpty(Information))
                {
                    error = "Information about the state is required!";
                }
                if (columnName == "RenovationLevel" && !RadioButton1IsChecked && !RadioButton2IsChecked && !RadioButton3IsChecked && !RadioButton4IsChecked && !RadioButton5IsChecked)
                {
                    error = "Must be selected one option for level of renovation urgency!";
                }
                return error;
            }

        }

        private readonly string[] _validatedProperties = { "Information", "RenovationLevel" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

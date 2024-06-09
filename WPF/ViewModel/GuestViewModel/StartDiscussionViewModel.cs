using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.WPF.DTOs;
using BookingApp.WPF.View;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace BookingApp.WPF.ViewModel.GuestViewModel
{
    public class StartDiscussionViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private readonly UserDTO user;
        private readonly LocationService _locationService = new();
        private readonly ForumService _forumService = new();
        private readonly CommentService _commentService = new();
        private readonly AccommodationReservationService _accommodationReservationService = new();

        private NavigationService Navigate { get; set; }

        public RelayCommand CreateForumCommand { get; set; }
        public ObservableCollection<string> Countries { get; set; }
        public ObservableCollection<string> Cities { get; set; }

        private string _selectedCountry;
        public string SelectedCountry
        {
            get { return _selectedCountry; }
            set
            {
                if (_selectedCountry != value)
                {
                    _selectedCountry = value;
                    OnPropertyChanged();
                    UpdateCities();
                }
            }
        }

        private string _selectedCity;
        public string SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                if (_selectedCity != value)
                {
                    _selectedCity = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Id { get; set; }
        public int CommentId { get; set; }

        private CommentDTO firstCommentDTO;
        public CommentDTO FirstCommentDTO
        {
            get { return firstCommentDTO; }
            set
            {
                if (firstCommentDTO != value)
                {
                    firstCommentDTO = value;
                    OnPropertyChanged(nameof(firstCommentDTO));
                }
            }
        }

        private bool _isDemoRunning = false;
        private DispatcherTimer _demoTimer;
        private int _demoStep = 0;

        public RelayCommand DemoCommand { get; set; }
        public RelayCommand StopDemoCommand { get; set; }

        private bool _isCountryComboBoxOpen;
        public bool IsCountryComboBoxOpen
        {
            get { return _isCountryComboBoxOpen; }
            set
            {
                if (_isCountryComboBoxOpen != value)
                {
                    _isCountryComboBoxOpen = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isCityComboBoxOpen;
        public bool IsCityComboBoxOpen
        {
            get { return _isCityComboBoxOpen; }
            set
            {
                if (_isCityComboBoxOpen != value)
                {
                    _isCityComboBoxOpen = value;
                    OnPropertyChanged();
                }
            }
        }

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case "FirstCommentDTO.Text":
                        if (string.IsNullOrWhiteSpace(FirstCommentDTO.Text))
                            error = "Please enter comment";
                        break;
                    case "SelectedCountry":
                        if (string.IsNullOrWhiteSpace(SelectedCountry))
                            error = "Please choose country";
                        break;
                    case "SelectedCity":
                        if (string.IsNullOrWhiteSpace(SelectedCity))
                            error = "Please choose city";
                        break;
                }
                return error;
            }
        }

        public string Error => null;

        public StartDiscussionViewModel(ForumService forumService, LocationService locationService, UserDTO user, NavigationService navigationService)
        {
            this.user = user;
            Navigate = navigationService;

            CreateForumCommand = new RelayCommand(Execute_CreateForumCommand, CanExecute_Command);

            FirstCommentDTO = new CommentDTO();

            InitializeCollections();
            InitializeCombobox();

            DemoCommand = new RelayCommand(StartDemo_Click);
            StopDemoCommand = new RelayCommand(StopDemo_Click);
            _demoTimer = new DispatcherTimer();
            _demoTimer.Interval = TimeSpan.FromSeconds(1.5);
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
                    OpenCountryComboBox();
                    break;
                case 1:
                    SelectCountry();
                    break;
                case 2:
                    CloseCountryComboBox();
                    break;
                case 3:
                    OpenCityComboBox();
                    break;
                case 4:
                    SelectCity();
                    break;
                case 5:
                    CloseCityComboBox();
                    break;
                case 6:
                    CommentFill();
                    break;
                case 7:
                    DemoOpenForum();
                    break;

            }

            _demoStep = (_demoStep + 1) % 10;
        }

        private void OpenCountryComboBox()
        {
            IsCountryComboBoxOpen = true;
        }

        private void CloseCountryComboBox()
        {
            IsCountryComboBoxOpen = false;
        }

        private void OpenCityComboBox()
        {
            IsCityComboBoxOpen = true;
        }

        private void CloseCityComboBox()
        {
            IsCityComboBoxOpen = false;
        }

        private void SelectCountry()
        {
            SelectedCountry = "Turska";
        }

        private void SelectCity()
        {
            SelectedCity = "Istanbul";
        }


        private void CommentFill()
        {
            FirstCommentDTO.Text = "Moj omiljeni grad!";
        }

        
        private void DemoOpenForum()
        {
             // Execute_CreateForumCommand(null);
        }
        

        private void UpdateCities()
        {
            if (!string.IsNullOrEmpty(SelectedCountry))
            {
                var cities = _locationService.GetCitiesByCountry(SelectedCountry);
                Cities.Clear();
                Cities.Add("City"); 
                foreach (var city in cities)
                {
                    Cities.Add(city);
                }
                SelectedCity = Cities[0];
                OnPropertyChanged(nameof(Cities));
                OnPropertyChanged(nameof(SelectedCity));
            }
        }

        private void InitializeCollections()
        {
            Countries = new ObservableCollection<string>();
            Cities = new ObservableCollection<string>();
        }
        private void InitializeCombobox()
        {
            InitializeCountries();
        }

        private void InitializeCountries()
        {
            foreach (var country in _locationService.GetAllCountries().OrderBy(c => c))
            {
                Countries.Add(country);
            }
        }
        public void UpdateCityComboBox()
        {
            Cities.Clear();
            foreach (string city in _locationService.GetCitiesByCountry(SelectedCountry).OrderBy(c => c))
            {
                Cities.Add(city);
            }
        }
        private LocationDTO GetLocation()
        {
            return _locationService.GetLocation(SelectedCountry, SelectedCity);
        }

        private bool CanExecute_Command(object obj)
        {
            return !string.IsNullOrWhiteSpace(FirstCommentDTO.Text) &&
               !string.IsNullOrWhiteSpace(SelectedCountry) &&
               !string.IsNullOrWhiteSpace(SelectedCity);
        }

        public void Execute_CreateForumCommand(object obj)
        {
            var location = GetLocation();
            if (_forumService.ForumExists(location.Id))
            {
                MessageBox.Show("Forum for this location already exists. View ExploreForums section");
                return;
            }

            var messageBoxResult = MessageBox.Show($"Are you sure you want to create new forum?", "Create Forum Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                ForumDTO forum = new ForumDTO
                {
                    Id = Id,
                    LocationId = location.Id,
                    GuestId = user.Id,
                    CreationTime = DateTime.Now,
                    IsOpened = true,
                    GuestCommentsCount = 0,
                    OwnerCommentsCount = 0,
                    IsVeryUseful = false
                };

                var guestHasReservation = _accommodationReservationService.HasGuestVisitedLocation(user.Id, location.Id);
             
                var savedForum = _forumService.Save(forum.ToForum());

                CommentDTO comment = new CommentDTO
                {
                    Id = CommentId,
                    CreationTime = DateTime.Now,
                    Text = FirstCommentDTO.Text,
                    User = user.ToUser(),
                    ForumId = savedForum.Id,
                    ReportNumber = 0
                };
                _commentService.Save(comment.ToComment());

                if (guestHasReservation)
                {
                    var updatedForum = new ForumDTO
                    {
                        Id = savedForum.Id,
                        LocationId = savedForum.LocationId,
                        GuestId = savedForum.GuestId,
                        CreationTime = savedForum.CreationTime,
                        IsOpened = savedForum.IsOpened,
                        GuestCommentsCount = savedForum.GuestCommentsCount + 1,
                        OwnerCommentsCount = savedForum.OwnerCommentsCount,
                        IsVeryUseful = savedForum.IsVeryUseful
                    };

                    _forumService.Update(updatedForum.ToForum());
                }

                MessageBox.Show("Forum created successfully!");
                Navigate.Navigate(new Forums(user, Navigate));
            }
        }


        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}

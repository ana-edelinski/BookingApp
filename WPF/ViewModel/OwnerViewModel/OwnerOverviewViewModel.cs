using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.View.OwnerView;
using BookingApp.WPF.DTOs;
using BookingApp.WPF.View.OwnerView;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.OwnerViewModel
{
    public class OwnerOverviewViewModel : NotifyPropertyChangedImplemented
    {
        private readonly App app;
        private const string ENG = "en-US";

        private readonly GuestRatingService guestRatingService = new();
        private readonly ForumService forumService = new();
        private readonly LocationService locationService = new();
        
        private NavigationService Navigate { get; set; }
        public ICommand NavigateToPageCommand { get; private set; }

        public UserDTO User { get; set; }

        private List<string> notifications;
        public List<string> Notifications
        {
            get { return notifications; }
            set
            {
                if(notifications != value)
                {
                    notifications = value;
                    OnPropertyChanged(nameof(notifications));
                }
            }
        }

        private void GetRecommendations()
        {
            if(locationService.GetRecommendationsForReservations().Item1.Id == locationService.GetRecommendationsForOccupancy().Item1.Id)
            {
                Notifications.Add($"Best location: {locationService.GetRecommendationsForReservations().Item1.City}, {locationService.GetRecommendationsForReservations().Item1.Country}");
            }
            else
            {
                Notifications.Add($"Best location (reservations): {locationService.GetRecommendationsForReservations().Item1.City}, {locationService.GetRecommendationsForReservations().Item1.Country}");
                Notifications.Add($"Best location (occupancy): {locationService.GetRecommendationsForOccupancy().Item1.City}, {locationService.GetRecommendationsForOccupancy().Item1.Country}");
            }
            
            if(locationService.GetRecommendationsForReservations().Item2.Id == locationService.GetRecommendationsForOccupancy().Item2.Id)
            {
                Notifications.Add($"Worst location: {locationService.GetRecommendationsForOccupancy().Item2.City}, {locationService.GetRecommendationsForOccupancy().Item2.Country}");
            }
            else
            {
                Notifications.Add($"Worst location (reservations): {locationService.GetRecommendationsForReservations().Item2.City}, {locationService.GetRecommendationsForReservations().Item2.Country}");
                Notifications.Add($"Worst location (occupancy): {locationService.GetRecommendationsForOccupancy().Item2.City}, {locationService.GetRecommendationsForOccupancy().Item2.Country}");
            }
        }
        private void GetNotifications()
        {
            if (guestRatingService.GetUnratedGuests().Any()) Notifications.Add("You have guests to rate");
            foreach (ForumDTO forumDTO in forumService.GetAllDTO())
            {
                if (forumDTO.IsNotification())
                {
                    LocationDTO locationDTO = locationService.GetByIdDTO(forumDTO.LocationId);
                    Notifications.Add($"New forum: {locationDTO.City}, {locationDTO.Country}");
                }
            }

            GetRecommendations();
        }

        private void NavigateToPage(object paramter)
        {

            switch (paramter)
            {
                case "Accommodations":
                    Navigate.Navigate(new Accommodations(User, Navigate));
                    break;
                case "UnratedGuests":
                    Navigate.Navigate(new UnratedGuests(User, Navigate));
                    break;
                case "Reviews":
                    Navigate.Navigate(new ViewReviews(User, Navigate));
                    break;
                case "AllRenovations":
                    Navigate.Navigate(new Renovations(User));
                    break;
                case "Forums":
                    Navigate.Navigate(new Forums(User, Navigate));
                    break;
                case "MoveRequests":
                    Navigate.Navigate(new ReservationsManagement(User));
                    break;
                case "AllReservations":
                    Navigate.Navigate(new Reservations(User));
                    break;
            }
        }

        public OwnerOverviewViewModel(UserDTO user, NavigationService navigationService) 
        {
            User = user;

            Navigate = navigationService;
            NavigateToPageCommand = new RelayCommand(NavigateToPage);
            Navigate.Navigate(new Accommodations(User, Navigate));

            app = (App)Application.Current;
            app.ChangeLanguage(ENG);

            Notifications = new();
            GetNotifications();
        }
    }
}

using BookingApp.Domain.Model;
using BookingApp.View;
using BookingApp.WPF.View.GuestView;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using BookingApp.WPF.DTOs;
using BookingApp.WPF.ViewModel.GuestViewModel;
using System.Windows.Input;
using BookingApp.Applications.UtilityInterfaces;
using System.Threading.Tasks;
using System.Windows.Threading;
using System;

namespace BookingApp.WPF.View
{
    /// <summary>
    /// Interaction logic for GuestOverview.xaml
    /// </summary>
    public partial class GuestOverview : Window
    {
        private readonly UserDTO user;
        private readonly NotificationsViewModel notificationsViewModel;
        private AnywhereAnytimeViewModel anywhereAnytimeViewModel;
        private RateRecommendViewModel rateRecommendViewModel;
        private RenovationRecommendationViewModel renovationRecommendationViewModel;
        private StartDiscussionViewModel startDiscussionViewModel;

        private AnywhereAnytime anywhereAnytimePage;
        private RenovationRecommendation renovationRecommendationPage;
        private StartDiscussion startDiscussionPage;

        private bool _isDemoRunning = false;
        private DispatcherTimer _demoTimer;
        private int _demoStep = 0;


        public GuestOverview(UserDTO user)
        {
            InitializeComponent();
            DataContext = user;
            this.user = user;

            notificationsViewModel = new NotificationsViewModel(user);
            anywhereAnytimeViewModel = new AnywhereAnytimeViewModel(new UserDTO(), NavigationService.GetNavigationService(GuestOverviewFrame));
            rateRecommendViewModel = new RateRecommendViewModel(new OwnerAccommodationRatingDTO(), user);
            renovationRecommendationViewModel = new(rateRecommendViewModel);
            startDiscussionViewModel = new(new Applications.UseCases.ForumService(), new Applications.UseCases.LocationService(), user, NavigationService.GetNavigationService(GuestOverviewFrame));
            NotificationsImage.DataContext = notificationsViewModel;
            notificationsViewModel.CheckRescheduleRequestsStatus();

            anywhereAnytimePage = new AnywhereAnytime(user, NavigationService.GetNavigationService(GuestOverviewFrame));
            renovationRecommendationPage = new RenovationRecommendation(rateRecommendViewModel);
            startDiscussionPage = new StartDiscussion(new Applications.UseCases.ForumService(), new Applications.UseCases.LocationService(), user, NavigationService.GetNavigationService(GuestOverviewFrame));


            AccommodationsAndReservationsView();

            _demoTimer = new DispatcherTimer();
            _demoTimer.Interval = TimeSpan.FromSeconds(2);
        }

        private void AccommodationsAndReservationsView()
        {
            GuestOverviewFrame.Navigate(new AccommodationsAndReservations(user));
        }

        private void AccommodationsButton_Click(object sender, RoutedEventArgs e)
        {
            GuestOverviewFrame.Navigate(new AccommodationsAndReservations(user));
        }

        private void ReviewsButton_Click(object sender, RoutedEventArgs e)
        {
            GuestOverviewFrame.Navigate(new Reviews(user));
        }

        private void ForumsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService navigationService = GuestOverviewFrame.NavigationService;
            GuestOverviewFrame.Navigate(new Forums(user, navigationService));
        }

        private void NavigateToAnywhereAnytime()
        {
            GuestOverviewFrame.Content = anywhereAnytimePage;
        }

        private void NavigateToRenovationRecommendation()
        {
            GuestOverviewFrame.Content = renovationRecommendationPage;
        }

        private void NavigateToStartDiscussion()
        {
            GuestOverviewFrame.Content = startDiscussionPage;
        }

        private void Notifications_Click(object sender, RoutedEventArgs e)
        {
            GuestOverviewFrame.Navigate(new Notifications(user));
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            OptionsMenu.IsOpen = true;
        }

        private void MyAccount_Click(object sender, RoutedEventArgs e)
        {
            GuestOverviewFrame.Navigate(new MyAccount(user));
        }

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new();
            signInForm.Show();
            Close();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (GuestOverviewFrame.NavigationService.CanGoBack)
            {
                GuestOverviewFrame.NavigationService.GoBack();
            }
        }
        
        private void GuestOverviewFrame_Navigated(object sender, NavigationEventArgs e)
        {
            GuestOverviewFrame.Navigated -= GuestOverviewFrame_Navigated;

            if (e.Content is IDemoPage newDemoPage)
            {
                newDemoPage.StartDemo();
            }
            else if (e.Content == renovationRecommendationPage)
            {
                NavigateToAnywhereAnytime();
            }
            else if (e.Content == anywhereAnytimePage)
            {
                NavigateToStartDiscussion();
            }
        }


        private async void StartDemoMode()
        {
            while (_isDemoRunning)
            {
                if (!_isDemoRunning) return;
                ResetPages();
                NavigateToRenovationRecommendation();
                (renovationRecommendationPage as IDemoPage)?.StartDemo();
                await Task.Delay(5000);

                if (!_isDemoRunning) return;
                ResetPages();
                NavigateToAnywhereAnytime();
                (anywhereAnytimePage as IDemoPage)?.StartDemo();
                await Task.Delay(10000);

                if (!_isDemoRunning) return;
                ResetPages();
                NavigateToStartDiscussion();
                (startDiscussionPage as IDemoPage)?.StartDemo();
                await Task.Delay(15000); 
            }
        }

        private void DemoModeStart_Click(object sender, RoutedEventArgs e)
        {
            _isDemoRunning = true;
            _demoStep = 0;
            //MessageBox.Show("Starting Demo");
            _demoTimer.Start();

            if (!_isDemoRunning) return;
            StartDemoMode();
        }

        private void DemoModeStop_Click(object sender, RoutedEventArgs e)
        {
            _isDemoRunning = false;
            _demoTimer.Stop();

            (renovationRecommendationPage as IDemoPage)?.StopDemo();
            (anywhereAnytimePage as IDemoPage)?.StopDemo();
            (startDiscussionPage as IDemoPage)?.StopDemo();

        }

        private void ResetPages()
        {
            renovationRecommendationPage = new RenovationRecommendation(rateRecommendViewModel);
            anywhereAnytimePage = new AnywhereAnytime(user, NavigationService.GetNavigationService(GuestOverviewFrame));
            startDiscussionPage = new StartDiscussion(new Applications.UseCases.ForumService(), new Applications.UseCases.LocationService(), user, NavigationService.GetNavigationService(GuestOverviewFrame));
        }




    }
}
using BookingApp.Domain.Model;
using BookingApp.WPF.DTOs;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BookingApp.WPF.View.GuestView;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for AccommodationsAndReservations.xaml
    /// </summary>
    public partial class AccommodationsAndReservations : Page
    {
        private readonly UserDTO user;

        public AccommodationsAndReservations(UserDTO user)
        {
            InitializeComponent();
            this.user = user;
        }

        private void Discover_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AccommodationViewSearch(user));
        }

        private void MyReservations_Click(object sender, RoutedEventArgs e)
        {
            NavigationService navigationService = NavigationService.GetNavigationService(this);
            MainFrame.Navigate(new MyReservations(user));
        }

        private void RequestStatus_Click(object sender,  EventArgs e)
        {
            NavigationService navigationService = NavigationService.GetNavigationService(this);
            MainFrame.Navigate(new RescheduleReservationStatus(user));
        }

        private void AnywhereAnyTime_Click(object sender, EventArgs e)
        {
            //todo
            NavigationService navigationService = NavigationService.GetNavigationService(this);
            MainFrame.Navigate(new AnywhereAnytime(user, navigationService));
        }
    }
}
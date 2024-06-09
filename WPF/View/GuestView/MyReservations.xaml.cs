using BookingApp.Applications.UseCases;
using BookingApp.Domain.Model;
using BookingApp.WPF.DTOs;
using BookingApp.WPF.View.GuestView;
using BookingApp.WPF.ViewModel.GuestViewModel;
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

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for MyReservations.xaml
    /// </summary>
    public partial class MyReservations : Page
    {
        public MyReservationsViewModel ViewModel { get; set; }

        private readonly UserDTO user;

        public MyReservations(UserDTO user)
        {
            InitializeComponent();
            this.user = user;
            NavigationService navigationService = NavigationService.GetNavigationService(this);
            ViewModel = new MyReservationsViewModel(user, navigationService);
            DataContext = ViewModel;
        }
        private void ReschedulePage_CLick(object sender, RoutedEventArgs e) //TODO: Uraditi preko komande ovo
        {
            var button = sender as Button;
            var reservation = button.DataContext as AccommodationReservationDTO;
            if (reservation != null)
            {
                ViewModel.SelectedReservation = reservation;
                NavigationService?.Navigate(new RescheduleReservation(ViewModel));
            }
        }
    }
}

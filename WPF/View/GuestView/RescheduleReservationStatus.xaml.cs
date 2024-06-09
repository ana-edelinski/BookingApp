using BookingApp.WPF.DTOs;
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

namespace BookingApp.WPF.View.GuestView
{
    /// <summary>
    /// Interaction logic for RescheduleReservationStatus.xaml
    /// </summary>
    public partial class RescheduleReservationStatus : Page
    {
        public RescheduleReservationStatusViewModel ViewModel { get; set; }
        private readonly UserDTO user = new();
        private readonly AccommodationReservationDTO accommodationReservation = new();
        public RescheduleReservationStatus(UserDTO loggedInUser)
        {
            InitializeComponent();
            user = loggedInUser;
            NavigationService navigationService = NavigationService.GetNavigationService(this);
            ViewModel = new RescheduleReservationStatusViewModel(user, accommodationReservation);
            DataContext = ViewModel;
        }
    }
}

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
using BookingApp.Applications.UseCases;
using BookingApp.Domain.Model;
using BookingApp.WPF.DTOs;
using BookingApp.WPF.ViewModel.GuestViewModel;

namespace BookingApp.WPF.View.GuestView
{
    /// <summary>
    /// Interaction logic for Availability.xaml
    /// </summary>
    public partial class Availability : Page
    {
        public AvailabilityViewModel ViewModel { get; set; }

        private readonly UserDTO user;

        public Availability(AccommodationDTO SelectedAvailableAccommodation, AccommodationService accommodationService, AccommodationReservationService accommodationReservationService, int? numberOfDays, int? numberOfGuests, UserDTO user)
        {
            InitializeComponent();
            this.user = user;
            ViewModel = new AvailabilityViewModel(SelectedAvailableAccommodation, accommodationService, accommodationReservationService, numberOfDays, numberOfGuests, user);
            DataContext = ViewModel;
        }
    }
}

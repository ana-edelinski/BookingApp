using BookingApp.WPF.DTOs;
using BookingApp.WPF.ViewModel.OwnerViewModel;
using System.Windows.Controls;

namespace BookingApp.WPF.View.OwnerView
{
    public partial class Reservations : Page
    {
        public Reservations(UserDTO user)
        {
            InitializeComponent();
            DataContext = new ReservationsViewModel(user);
        }
    }
}

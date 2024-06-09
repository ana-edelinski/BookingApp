using System.Windows.Controls;
using BookingApp.WPF.DTOs;
using BookingApp.WPF.ViewModel.OwnerViewModel;

namespace BookingApp.View.OwnerView
{
    public partial class ReservationsManagement : Page
    {
        public ReservationsManagement(UserDTO user) 
        {
            InitializeComponent();
            DataContext = new ReservationsManagementViewModel(user);

           
        }
    }
}

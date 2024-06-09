using BookingApp.WPF.DTOs;
using BookingApp.WPF.ViewModel.OwnerViewModel;
using System.Windows.Controls;

namespace BookingApp.View.OwnerView
{
    public partial class Renovations : Page
    {
        public Renovations(UserDTO user)
        {
            InitializeComponent();
            DataContext = new RenovationsViewModel(user);
        }
    }
}

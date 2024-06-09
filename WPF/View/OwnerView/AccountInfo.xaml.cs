using BookingApp.WPF.DTOs;
using BookingApp.WPF.ViewModel.OwnerViewModel;
using System.Windows.Controls;

namespace BookingApp.View.OwnerView
{
    public partial class AccountInfo : Page
    {
        public AccountInfo(UserDTO user)
        {
            InitializeComponent();

            DataContext = new AccountInfoViewModel(user);
        }

    }
}

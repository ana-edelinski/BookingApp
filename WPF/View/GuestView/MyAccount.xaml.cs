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
using BookingApp.WPF.DTOs;
using BookingApp.WPF.ViewModel.GuestViewModel;

namespace BookingApp.WPF.View.GuestView
{
    /// <summary>
    /// Interaction logic for MyAccount.xaml
    /// </summary>
    public partial class MyAccount : Page
    {
        public MyAccountViewModel ViewModel { get; set; }
        public MyAccount(UserDTO user)
        {
            InitializeComponent();
            ViewModel = new MyAccountViewModel(user);
            DataContext = ViewModel;
        }

        private void MyAccount_OnLoaded(object sender, RoutedEventArgs e)
        {
           ViewModel.CheckAndUpdateSuperGuestStatus();
        }
    }
}

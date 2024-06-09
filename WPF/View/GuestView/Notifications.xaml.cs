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
    /// Interaction logic for Notifications.xaml
    /// </summary>
    public partial class Notifications : Page
    {
        private readonly UserDTO user = new();
        public NotificationsViewModel ViewModel { get; set; }
        public Notifications(UserDTO loggedInUser)
        {
            InitializeComponent();
            user = loggedInUser;
            ViewModel = new NotificationsViewModel(user);
            DataContext = ViewModel;
        }

        private void Notifications_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is NotificationsViewModel viewModel)
            {
                viewModel.CheckRescheduleRequestsStatus();
            }
        }
    }
}

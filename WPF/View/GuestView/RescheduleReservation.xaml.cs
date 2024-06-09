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
    /// Interaction logic for RescheduleReservation.xaml
    /// </summary>
    public partial class RescheduleReservation : Page
    {
        public RescheduleReservationViewModel ViewModel { get; set; }
        public RescheduleReservation(MyReservationsViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = new RescheduleReservationViewModel(viewModel);
            DataContext = ViewModel;
        }
    }
}

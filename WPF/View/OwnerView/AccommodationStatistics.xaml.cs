using BookingApp.WPF.DTOs;
using BookingApp.WPF.ViewModel.OwnerViewModel;
using System.Windows;
using System.Windows.Controls;

namespace BookingApp.WPF.View.OwnerView
{
    public partial class AccommodationStatistics : Page
    {
        public AccommodationStatistics(AccommodationDTO accommodation)
        {
            InitializeComponent();
            DataContext = new AccommodationStatisticsViewModel(accommodation);
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}

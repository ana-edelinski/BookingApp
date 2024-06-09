using BookingApp.WPF.ViewModel.OwnerViewModel;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.OwnerView
{
    public partial class ReviewDetails : Page
    {
        public ReviewDetails(dynamic SelectedItem, NavigationService navigationService)
        {
            InitializeComponent();
            DataContext = new ReviewDetailsViewModel(SelectedItem,navigationService);
        }
    }
}

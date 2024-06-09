using BookingApp.WPF.DTOs;
using System.Windows.Controls;
using BookingApp.WPF.ViewModel.OwnerViewModel;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.OwnerView
{
    public partial class ForumComments : Page
    {
        public ForumComments(dynamic selectedItem, UserDTO user, NavigationService navigationService)
        {
            InitializeComponent();
            DataContext = new ForumCommentsViewModel(selectedItem,user, navigationService);
        }
    }
}

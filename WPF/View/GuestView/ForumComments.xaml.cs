using BookingApp.Applications.UseCases;
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
    /// Interaction logic for ForumComments.xaml
    /// </summary>
    public partial class ForumComments : Page
    {
        public ForumCommentsViewModel ViewModel { get; set; }
        public ForumComments(ForumDTO selectedForum, CommentService commentService, UserDTO user, NavigationService navigationService)
        {
            InitializeComponent();
            ViewModel = new ForumCommentsViewModel(selectedForum, commentService, user, navigationService);
            DataContext = ViewModel;
        }
    }
}

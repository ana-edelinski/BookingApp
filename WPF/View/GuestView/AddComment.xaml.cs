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
    /// Interaction logic for AddComment.xaml
    /// </summary>
    public partial class AddComment : Page
    {
        public AddCommentViewModel ViewModel { get; set; }
        public AddComment(ForumDTO selectedForum, CommentService commentService, UserDTO user)
        {
            InitializeComponent();
            ViewModel = new AddCommentViewModel(selectedForum, commentService, user);
            DataContext = ViewModel;
        }
    }
}

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
    /// Interaction logic for MyReviews.xaml
    /// </summary>
    public partial class MyReviews : Page
    {
        public MyReviewsViewModel ViewModel { get; set; }
        private readonly UserDTO user = new();  //

        public MyReviews(UserDTO loggedInUser)
        {
            InitializeComponent();
            user = loggedInUser;
            ViewModel = new MyReviewsViewModel(user);
            DataContext = ViewModel;
        }
    }
}

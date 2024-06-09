using BookingApp.Domain.Models;
using BookingApp.Observer;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using BookingApp.Domain.Model;
using BookingApp.WPF.DTOs;
using BookingApp.WPF.ViewModel.OwnerViewModel;
using BookingApp.WPF.View.GuestView;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for Reviews.xaml
    /// </summary>
    public partial class Reviews : Page
    {

        private readonly UserDTO user;
        public Reviews(UserDTO user)
        {
            InitializeComponent();
            this.user = user;
        }

        private void RateReccommend_Click(object sender, RoutedEventArgs e)
        {
            NavigationService navigationService = NavigationService.GetNavigationService(this);
            MainFrame.Navigate(new RecentStays(user, navigationService));  //viewmodel??
        }

        private void MyReviews_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MyReviews(user));
        }

       
    }
}

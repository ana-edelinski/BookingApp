using BookingApp.Applications.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.Repository;
using BookingApp.View;
using BookingApp.View.OwnerView;
using BookingApp.WPF.DTOs;
using BookingApp.WPF.ViewModel.GuestViewModel;
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

namespace BookingApp.WPF.View.GuestView
{
    /// <summary>
    /// Interaction logic for RateRecommend.xaml
    /// </summary>
    public partial class RateRecommend : Page
    {
        public RateRecommendViewModel ViewModel { get; set; }
        private readonly UserDTO user = new();

        public RateRecommend(OwnerAccommodationRatingDTO selectedUnratedAccommodation, UserDTO loggedInUser)
        {
            InitializeComponent();
            user = loggedInUser;
            ViewModel = new RateRecommendViewModel(selectedUnratedAccommodation, loggedInUser);
            DataContext = ViewModel;
            
        }

        private void Rate_Click(object sender, RoutedEventArgs e)
        {
            RateAccommodationAndOwner rateAccommodationAndOwnerPage = new RateAccommodationAndOwner(ViewModel);
            NavigationService?.Navigate(rateAccommodationAndOwnerPage);
        }

        private void RenovationRecommendation_Click(object sender, RoutedEventArgs e)
        {
            RenovationRecommendation renovationRecommendationPage = new RenovationRecommendation(ViewModel);
            NavigationService?.Navigate(renovationRecommendationPage);


        }
    }
}
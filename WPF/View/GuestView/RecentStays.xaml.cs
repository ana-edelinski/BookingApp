using BookingApp.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
using BookingApp.Repository;
using BookingApp.Repositories;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.View.OwnerView;
using BookingApp.WPF.View.GuestView;
using BookingApp.Domain.Models;
using BookingApp.Applications.UseCases;
using BookingApp.WPF.DTOs;
using BookingApp.WPF.ViewModel.GuestViewModel;
using BookingApp.WPF.ViewModel.OwnerViewModel;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for RecentStays.xaml
    /// </summary>
    public partial class RecentStays : Page
    {
        public RecentStaysViewModel ViewModel { get; set; }

        private readonly UserDTO user;
        private AccommodationDTO accommodation;

        public RecentStays(UserDTO user, NavigationService navigationService)
        {
            InitializeComponent();
            this.user = user;
            ViewModel = new RecentStaysViewModel(user, navigationService);
            DataContext = ViewModel;

        }

    }
}
using BookingApp.Applications.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.Repository;
using BookingApp.WPF.DTOs;
using BookingApp.WPF.ViewModel.GuestViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
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
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for RateAccommodationAndOwner.xaml
    /// </summary>
    public partial class RateAccommodationAndOwner : Page
    {
        public RateAccommodationAndOwnerViewModel ViewModel { get; set; }

        private readonly UserDTO user = new();

        public RateAccommodationAndOwner(RateRecommendViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = new RateAccommodationAndOwnerViewModel(viewModel);
            DataContext = ViewModel;
        }
    }
}
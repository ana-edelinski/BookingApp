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
using BookingApp.Applications.UtilityInterfaces;
using BookingApp.WPF.DTOs;
using BookingApp.WPF.ViewModel.GuestViewModel;

namespace BookingApp.WPF.View.GuestView
{
    /// <summary>
    /// Interaction logic for AnywhereAnytime.xaml
    /// </summary>
    public partial class AnywhereAnytime : Page, IDemoPage
    {
        public AnywhereAnytimeViewModel ViewModel { get; set; }
        private readonly UserDTO user;
        public AnywhereAnytime(UserDTO user, NavigationService navigationService)
        {
            InitializeComponent();
            this.user = user;
            ViewModel = new AnywhereAnytimeViewModel(user, navigationService);
            DataContext = ViewModel;
        }

        public void StartDemo()
        {
            ViewModel.DemoCommand.Execute(null);
        }

        public void StopDemo()
        {
            ViewModel.StopDemoCommand.Execute(null);
        }
    }
}

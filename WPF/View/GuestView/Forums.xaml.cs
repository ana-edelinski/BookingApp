using BookingApp.WPF.DTOs;
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
namespace BookingApp.WPF.View;
using BookingApp.WPF.ViewModel.GuestViewModel;

/// <summary>
/// Interaction logic for Forums.xaml
/// </summary>
public partial class Forums : Page
{
    public ForumsViewModel ViewModel {  get; set; }
    private readonly UserDTO user;
    public Forums(UserDTO user, NavigationService navigationService)
    {
        InitializeComponent();
        this.user = user;
        ViewModel = new ForumsViewModel(user, navigationService);
        DataContext = ViewModel;
    }
}

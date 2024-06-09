using BookingApp.Repository;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Navigation;
using System;
using BookingApp.View.OwnerView;
using BookingApp.Domain.Model;
using BookingApp.Domain.Model.Enums;
using BookingApp.WPF.DTOs;
using BookingApp.WPF.View;
using System.Windows.Input;



namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    {

        private readonly UserRepository _repository;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SignInForm()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new UserRepository();
        }

        private void PasswordBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SignIn(sender, e);
                e.Handled = true;
            }
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            User user = _repository.GetByUsername(Username);
            if (user != null)
            {
                if (user.Password == txtPassword.Password)
                {
                    if (user.UserType == UserType.GUEST || user.UserType == UserType.SUPERGUEST)
                    {
                        GuestOverview guestOverview = new GuestOverview(new UserDTO(user));
                        guestOverview.Show();
                    }
                    else if (user.UserType == UserType.OWNER)
                    {
                        OwnerOverview ownerOverview = new OwnerOverview(new UserDTO(user));
                        ownerOverview.Show();
                    }
                    Close();
                }
                else
                {
                    MessageBox.Show("Wrong password!");
                }
            }
            else
            {
                MessageBox.Show("Wrong username!");
            }

        }
    }
}
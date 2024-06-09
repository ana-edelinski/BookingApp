using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using BookingApp.Applications.UseCases;
using BookingApp.Domain.Model.Enums;
using System.Windows;
using BookingApp.Applications.Utilities;
using BookingApp.View;
using BookingApp.WPF.DTOs;

namespace BookingApp.WPF.ViewModel.GuestViewModel
{
    public class MyAccountViewModel : INotifyPropertyChanged
    {
        private readonly AccommodationReservationService _accommodationReservationService = new();
        private readonly UserService _userService = new();
        public RelayCommand SignOutCommand { get; set; }

        private UserDTO guestDTO;
        public UserDTO GuestDTO
        {
            get { return guestDTO; }
            set
            {
                if (guestDTO != value)
                {
                    guestDTO = value;
                    OnPropertyChanged(nameof(GuestDTO));
                    CheckAndUpdateSuperGuestStatus();  
                }
            }
        }

        private Visibility isSuperGuestVisible;
        public Visibility IsSuperGuestVisible
        {
            get { return isSuperGuestVisible; }
            set
            {
                if (isSuperGuestVisible != value)
                {
                    isSuperGuestVisible = value;
                    OnPropertyChanged(nameof(IsSuperGuestVisible));
                }
            }
        }

        private Visibility regularUserText;
        public Visibility RegularUserText
        {
            get { return regularUserText; }
            set
            {
                if (regularUserText != value)
                {
                    regularUserText = value;
                    OnPropertyChanged(nameof(RegularUserText));
                }
            }
        }

        private Visibility superGuestText;
        public Visibility SuperGuestText
        {
            get { return superGuestText; }
            set
            {
                if (superGuestText != value)
                {
                    superGuestText = value;
                    OnPropertyChanged(nameof(SuperGuestText));
                }
            }
        }

        public MyAccountViewModel(UserDTO user)
        {
            GuestDTO = user;
            UpdateVisibilityBasedOnUserType();
            SignOutCommand = new RelayCommand(Execute_SignOutCommand, CanExecute_Command);
        }
        
        public void CheckAndUpdateSuperGuestStatus()
        {
            _userService.UpdateUserTitle(GuestDTO.Id);

            //UpdateUserTitleDTO
            DateTime oneYearAgo = DateTime.Now.AddYears(-1);
            int numberOfReservations = _accommodationReservationService.GetAll()
                .Where(i => i.GuestId == GuestDTO.Id && i.StartDate >= oneYearAgo)
                .Count();

            if (GuestDTO.UserType == UserType.GUEST && numberOfReservations >= 10)
            {
                GuestDTO.UserType = UserType.SUPERGUEST;
                GuestDTO.SuperGuestExpirationDate = DateTime.Now.AddDays(365);
                GuestDTO.BonusPoints = 5;

            }
            else if (GuestDTO.UserType == UserType.SUPERGUEST && numberOfReservations < 10)
            {
                GuestDTO.UserType = UserType.GUEST;
                GuestDTO.SuperGuestExpirationDate = null;
                GuestDTO.BonusPoints = null;
            }

            UpdateVisibilityBasedOnUserType();
        }

        private void UpdateVisibilityBasedOnUserType()
        {
            IsSuperGuestVisible = GuestDTO.UserType == UserType.SUPERGUEST ? Visibility.Visible : Visibility.Collapsed;
            SuperGuestText = GuestDTO.UserType == UserType.SUPERGUEST ? Visibility.Visible : Visibility.Collapsed;
            RegularUserText = GuestDTO.UserType == UserType.GUEST ? Visibility.Visible : Visibility.Collapsed;
        }

        private bool CanExecute_Command(object obj)
        {
            return true;
        }

        private void Execute_SignOutCommand(object obj)
        {
            SignInForm signInForm = new ();
            signInForm.Show();

            //TODO: kako da zatvorim ovde Window u okviru kog je frejm za ovaj Page

        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

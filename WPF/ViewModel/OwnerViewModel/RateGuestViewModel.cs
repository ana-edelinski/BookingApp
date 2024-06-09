using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.View.OwnerView;
using BookingApp.WPF.DTOs;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.OwnerViewModel
{
    public class RateGuestViewModel : NotifyPropertyChangedImplemented
    {
        private readonly GuestRatingService guestRatingService = new();
        private readonly CommentService commentService = new();

        public ICommand ConfirmCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand Cleanliness {  get; private set; }
        public ICommand RuleRespect {  get; private set; }

        private NavigationService Navigate { get; set; }
        private UserDTO User { get; set; }

        private GuestRatingDTO guestRatingDTO;
        public GuestRatingDTO GuestRatingDTO
        {
            get { return guestRatingDTO; }
            set
            {
                if(guestRatingDTO != value)
                {
                    guestRatingDTO = value;
                    OnPropertyChanged(nameof(GuestRatingDTO));
                }
            }
        }

        private CommentDTO commentDTO;
        public CommentDTO CommentDTO
        {
            get { return commentDTO; }
            set
            {
                if(commentDTO != value)
                {
                    commentDTO = value;
                    OnPropertyChanged(nameof(commentDTO));
                }
            }
        }

        
        private void ExecuteConfirmClick(object parameter) 
        {
            GuestRatingDTO.Validate();
            CommentDTO.Validate();
            if(GuestRatingDTO.IsValid && CommentDTO.IsValid)
            {
                GuestRatingDTO.IsRated = true;
                GuestRatingDTO.CommentId = commentService.Save(CommentDTO.ToComment()).Id;
                guestRatingService.Update(GuestRatingDTO.ToGuestRating());

                Navigate.Navigate(new UnratedGuests(User, Navigate));
            }
            
        }

        private void ExecuteCancelClick(object parameter)
        {
            Navigate.Navigate(new UnratedGuests(User, Navigate));
        }
        private void ExecuteCleanlinessClick(object parameter)
        {
            GuestRatingDTO.Cleanliness = Convert.ToInt32(parameter);
        }
        private void ExecuteRuleRespectClick(object parameter)
        {
            GuestRatingDTO.RuleRespect = Convert.ToInt32(parameter);
        }
        public RateGuestViewModel(UserDTO user, GuestRatingDTO selectedRating, UserDTO selectedGuest, NavigationService navigationService) 
        {
            User = user;
            GuestRatingDTO = selectedRating;
            CommentDTO = new();
            CommentDTO.User = selectedGuest.ToUser();
            Navigate = navigationService;

            CancelCommand = new RelayCommand(ExecuteCancelClick);
            ConfirmCommand = new RelayCommand(ExecuteConfirmClick);
            Cleanliness = new RelayCommand(ExecuteCleanlinessClick);
            RuleRespect = new RelayCommand(ExecuteRuleRespectClick);
        }
    }
}

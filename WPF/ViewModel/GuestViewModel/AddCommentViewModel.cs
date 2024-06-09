using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.Domain.Models;
using BookingApp.WPF.DTOs;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace BookingApp.WPF.ViewModel.GuestViewModel
{
    public class AddCommentViewModel : INotifyPropertyChanged
    {
        private readonly CommentService _commentService = new();
        private readonly ForumService _forumService = new();
        private readonly AccommodationService _accommodationService = new();
        private readonly AccommodationReservationService _accommodationReservationService = new();

        private readonly UserDTO user;

        public RelayCommand AddCommentCommand { get; set; }

        private CommentDTO _newComment;
        public CommentDTO NewComment
        {
            get => _newComment;
            set
            {
                if (value != _newComment)
                {
                    _newComment = value;
                    OnPropertyChanged();
                }
            }
        }

        private ForumDTO _selectedForum;
        public ForumDTO SelectedForum
        {
            get => _selectedForum;
            set
            {
                if (value != _selectedForum)
                {
                    _selectedForum = value;
                    OnPropertyChanged(nameof(SelectedForum));
                    OnPropertyChanged(nameof(CountryForum));
                    OnPropertyChanged(nameof(CityForum));
                }
            }
        }
        public string CountryForum
        {
            get => SelectedForum?.Country;
        }
        public string CityForum
        {
            get => SelectedForum?.City;
        }

        private bool CanExecute_Command(object obj)
        {
            return true;
        }

        private void Execute_AddCommentCommand(object obj)
        {
            var messageBoxResult = MessageBox.Show($"Would you like to add comment?", "Adding Comment Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                CommentDTO comment = new CommentDTO(DateTime.Now, NewComment.Text, user.ToUser(), SelectedForum.Id, 0);
                _commentService.Save(comment.ToComment());

                ForumDTO forumDTO = _forumService.GetByIdDTO(SelectedForum.Id);
                if (forumDTO != null)
                {
                    forumDTO.GuestCommentsCount++;
                    _forumService.Update(forumDTO);
                    MessageBox.Show("Comment added successfully.");
                }
            }
        }

        public AddCommentViewModel(ForumDTO selectedForum, CommentService commentService, UserDTO user)
        {
            SelectedForum = selectedForum;
            
            this.user = user;
            NewComment = new CommentDTO();

            AddCommentCommand = new RelayCommand(Execute_AddCommentCommand, CanExecute_Command);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

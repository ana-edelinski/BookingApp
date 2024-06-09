using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.WPF.DTOs;
using BookingApp.WPF.View.GuestView;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.GuestViewModel
{
    public class ForumCommentsViewModel : INotifyPropertyChanged
    {
        private readonly UserDTO user;
        
        private readonly CommentService _commentService = new();
        private readonly AccommodationReservationService _accommodationReservationService = new();

        public ObservableCollection<CommentDTO> AllComments { get; set; }
        private NavigationService Navigate { get; set; }


        public RelayCommand AddCommentCommand { get; set; }

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
                    OnPropertyChanged(nameof(IsForumOpened));
                    //FormComments();

                }
            }
        }

        public bool IsForumOpened
        {
            get => SelectedForum?.IsOpened ?? false; // Ako SelectedForum nije postavljen, smatramo da forum nije otvoren
        }

        private CommentDTO _additionalComment;
        public CommentDTO AdditionalComment
        {
            get => _additionalComment;
            set
            {
                if (value != _additionalComment)
                {
                    _additionalComment = value;
                    OnPropertyChanged();
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

        public void FormComments()
        {
            if (SelectedForum != null)
            {
                var comments = _commentService.GetCommentsByForumId(SelectedForum.Id);
                AllComments.Clear();
                foreach (var comment in comments)
                {
                    comment.Author = _commentService.GetCommentAuthorName(comment.Id);
                    comment.WasPresent = _accommodationReservationService.HasGuestVisitedLocation(comment.User.Id, SelectedForum.LocationId); 
                    AllComments.Add(comment);
                }
            }
        }

        private bool CanExecute_Command(object obj)
        {
            return IsForumOpened;
        }

        private void Execute_AddCommentCommand(object obj)
        {
            Navigate.Navigate(new AddComment(SelectedForum, _commentService, user));
        }

        public ForumCommentsViewModel(ForumDTO selectedForum, CommentService commentService, UserDTO user, NavigationService navigationService)
        {
            SelectedForum = selectedForum;
            _commentService = commentService;

            this.user = user;
            Navigate = navigationService;

            AllComments = new ObservableCollection<CommentDTO>(commentService.GetCommentsByForumId(selectedForum.Id));
            AdditionalComment = new CommentDTO();

            AddCommentCommand = new RelayCommand(Execute_AddCommentCommand, CanExecute_Command);

            FormComments();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

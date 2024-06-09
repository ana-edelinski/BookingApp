using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model.Enums;
using BookingApp.WPF.DTOs;
using BookingApp.WPF.View.GuestView;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.GuestViewModel
{
    public class ExploreForumsViewModel : INotifyPropertyChanged
    {
        private readonly UserDTO user;
        private readonly LocationService _locationService = new();
        private readonly ForumService _forumService = new();
        private readonly CommentService _commentService = new();

        public ObservableCollection<ForumDTO> AllForums { get; set; }
        public ForumDTO SelectedForum { get; set; }
        private NavigationService Navigate { get; set; }


        public RelayCommand CloseForumCommand { get; set; }
        public RelayCommand ViewCommentsCommand { get; set; }


        public ExploreForumsViewModel(UserDTO user, NavigationService navigationService)
        {
            this.user = user;
            Navigate = navigationService;
            SelectedForum = new ForumDTO();


            AllForums = new ObservableCollection<ForumDTO>();
            CloseForumCommand = new RelayCommand(Execute_CloseForumCommand, CanExecute_CloseForumCommand);
            ViewCommentsCommand = new RelayCommand(Execute_ViewCommentsCommand, CanExecute_Command);

            FormForums();
        }

        public void FormForums()
        {
            AllForums.Clear();
            var forums = _forumService.GetAll()
                .Select(f =>
                {
                    var isVeryUseful = f.GuestCommentsCount >= 20 && f.OwnerCommentsCount >= 10;

                    return new ForumDTO
                    {
                        Id = f.Id,
                        LocationId = f.LocationId,
                        Country = _locationService.GetCountryByLocationId(f.LocationId),
                        City = _locationService.GetCityByLocationId(f.LocationId),
                        IsOpened = f.IsOpened,
                        IsVeryUseful = isVeryUseful,
                        GuestCommentsCount = f.GuestCommentsCount,
                        OwnerCommentsCount = f.OwnerCommentsCount,
                        GuestId = f.GuestId
                    };
                });

            AllForums = new ObservableCollection<ForumDTO>(forums);
        }





        private bool CanExecute_Command(object obj)
        {
            return true;
        }

        private bool CanExecute_CloseForumCommand(object obj)
        {
            if (obj is ForumDTO forum)
            {
                return forum.IsOpened;
            }
            return false;
        }

        private void Execute_CloseForumCommand(object obj)
        {
            var selectedForum = obj as ForumDTO;

            var firstComment = _commentService.GetFirstCommentByForumId(selectedForum.Id);

            if (firstComment == null || firstComment.User.Id != user.Id)
            {
                MessageBox.Show("You can only close forums that you created.");
                return;
            }

            var messageBoxResult = MessageBox.Show($"Are you sure you want to close forum?", "Close Forum Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                var forumToUpdate = _forumService.GetById(selectedForum.Id);
                forumToUpdate.IsOpened = false;
                _forumService.Update(forumToUpdate);
                selectedForum.IsOpened = false;
                MessageBox.Show("Forum closed successfully.");
                OnPropertyChanged(nameof(AllForums));   
            }
        }

        private void Execute_ViewCommentsCommand(object parameter)
        {
            if(parameter is ForumDTO selectedForum)
            {
                SelectedForum = selectedForum;
                Navigate.Navigate(new ForumComments(selectedForum, _commentService, user, Navigate));
            }            
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}

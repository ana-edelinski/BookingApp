using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.WPF.DTOs;
using BookingApp.WPF.View.GuestView;
using System.ComponentModel;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.GuestViewModel
{
    public class ForumsViewModel : INotifyPropertyChanged
    {
        private readonly UserDTO user;
        private readonly ForumService forumService = new();
        private readonly LocationService locationService = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        private NavigationService Navigate { get; set; }
        public RelayCommand StartDiscussionCommand {  get; set; }
        public RelayCommand MyDiscussionsCommand { get; set; }
        public ForumsViewModel(UserDTO user, NavigationService navigationService) 
        {
            this.user = user;
            Navigate = navigationService;

            StartDiscussionCommand = new RelayCommand(Execute_StartDiscussionCommand, CanExecute_Command);
            MyDiscussionsCommand = new RelayCommand(Execute_MyDiscussionsCommand, CanExecute_Command);
        }

        private bool CanExecute_Command(object obj)
        {
            return true;
        }

        private void Execute_StartDiscussionCommand(object obj)
        {
            Navigate.Navigate(new StartDiscussion(forumService, locationService, user, Navigate));
        }

        private void Execute_MyDiscussionsCommand(object obj)
        {
            Navigate.Navigate(new ExploreForums(user, Navigate));
        }
    }
}

using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.WPF.DTOs;
using BookingApp.WPF.View.OwnerView;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.OwnerViewModel
{
    public class ForumsViewModel : NotifyPropertyChangedImplemented
    {
        private readonly ForumService forumService = new();
        private readonly CommentService commentService = new();
        private readonly UserService userService = new ();
        private readonly LocationService locationService = new ();

        private NavigationService Navigate {  get; set; }
        public UserDTO User { get; set; }

        private IEnumerable<dynamic> forumsDataGrid;
        public IEnumerable<dynamic> ForumsDataGrid
        {
            get { return forumsDataGrid; }
            set
            {
                if(forumsDataGrid != value)
                {
                    forumsDataGrid = value;
                    OnPropertyChanged(nameof(forumsDataGrid));
                }
            }
        }

        private dynamic selectedItem;
        public dynamic SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if(selectedItem != value)
                {
                    selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));
                    Navigate.Navigate(new ForumComments(selectedItem, User, Navigate));
                }
            }

        }

        private void GetForumsDataGrid()
        {
            var objectData = from forum in forumService.GetAllDTO()
                             join location in locationService.GetAllDTO() on forum.LocationId equals location.Id
                             join creator in userService.GetAllDTO() on forum.GuestId equals creator.Id
                             select new
                             {
                                 Forum = forum,
                                 Location = location,
                                 Creator = creator,
                                 IsUseful = commentService.IsForumUseful(forum.Id)
                             };

            ForumsDataGrid = objectData;
        }

        public ForumsViewModel(UserDTO user, NavigationService navigationService) 
        {
            User = user;
            Navigate = navigationService;
            GetForumsDataGrid();
        }
    }
}

using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.View.OwnerView;
using BookingApp.WPF.DTOs;
using System.Linq;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.OwnerViewModel
{
    public class ForumCommentsViewModel
    {
        private readonly CommentService commentService = new();
        private readonly UserService userService = new();
        private readonly AccommodationService accommodationService = new();
        private readonly AccommodationReservationService accommodationReservationService = new();

        private NavigationService Navigate {  get; set; }
        public ICommand GoBackCommand {  get; private set; }
        public ICommand PostCommentClick { get; private set; }
        public ICommand ReportCommentClick { get; private set; }

        private readonly UserDTO userDTO;
        public ForumCommentsDTO ForumCommentsDTO { get; private set; }
        
        private void ExtractData(dynamic selectedItem)
        {
            ForumCommentsDTO.SelectedForum = (ForumDTO)(selectedItem.GetType().GetProperty("Forum")?.GetValue(selectedItem, null));
            ForumCommentsDTO.SelectedLocation = (LocationDTO)(selectedItem.GetType().GetProperty("Location")?.GetValue(selectedItem, null));
        }

        private void ExecutePostComment(object parameter)
        {
            ForumCommentsDTO.Comment.User.Id = userDTO.Id;
            ForumCommentsDTO.Comment.ForumId = ForumCommentsDTO.SelectedForum.Id;

            commentService.Save(ForumCommentsDTO.Comment.ToComment());
            ForumCommentsDTO.Comment = new();
        }

        private void ExecuteReportComment(object parameter)
        {
            var selectedItem = parameter;

            if(selectedItem != null)
            {
                ForumCommentsDTO.SelectedComment = (CommentDTO)(selectedItem.GetType().GetProperty("Comment")?.GetValue(selectedItem, null));

                ForumCommentsDTO.SelectedComment = commentService.IncrementReportNumber(ForumCommentsDTO.SelectedComment);
                commentService.Update(ForumCommentsDTO.SelectedComment.ToComment());

            }
        }

        private bool CheckAccommodationLocation(int id)
        {
            return accommodationService.GetAllForOwner(id).Any(a => a.LocationId == ForumCommentsDTO.SelectedLocation.Id);

        }

        private bool CheckGuest(UserDTO user)
        {
            return !accommodationReservationService.GetAllDTO().Any(ar => ar.GuestId == user.Id) && user.UserType != 0;
        }
        private void GetCommentsDataGrid()
        {
            var objectsData = from comment in commentService.GetAllDTO()
                              join creator in userService.GetAllDTO() on comment.User.Id equals creator.Id
                              where comment.ForumId == ForumCommentsDTO.SelectedForum.Id
                              select new
                              {
                                  Comment = comment,
                                  Creator = creator,
                                  IsOwners = CheckAccommodationLocation(creator.Id),
                                  GuestWasNotHere = CheckGuest(creator)
                              };

            ForumCommentsDTO.CommentsDataGrid = objectsData;
        }

       private void ExecuteGoBackCommand(object parameter)
        {
            Navigate.Navigate(new Forums(userDTO, Navigate));
        }
        public ForumCommentsViewModel(dynamic selectedItem, UserDTO user, NavigationService navigationService) 
        {
            userDTO = user;
            Navigate = navigationService;
            ForumCommentsDTO = new();
            ExtractData(selectedItem);
            ForumCommentsDTO.HasAccommodation = CheckAccommodationLocation(user.Id);

            GetCommentsDataGrid();

            PostCommentClick = new RelayCommand(ExecutePostComment);
            ReportCommentClick = new RelayCommand(ExecuteReportComment);
            GoBackCommand = new RelayCommand(ExecuteGoBackCommand);
        }
    }
}

using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model.Enums;
using BookingApp.WPF.DTOs;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using BookingApp.View.OwnerView;
using System.Linq;

namespace BookingApp.WPF.ViewModel.OwnerViewModel
{
    public class ReservationsManagementViewModel : NotifyPropertyChangedImplemented
    {
        private readonly ImageService imageService = new();
        private readonly AccommodationService accommodationService = new();
        private readonly AccommodationReservationService accommodationReservationService = new ();
        private readonly AccommodationReservationMoveRequestService accommodationReservationMoveRequestService = new();
        private readonly CommentService commentService = new ();
        private readonly UserService userService = new ();

        private readonly UserDTO user;
        public ICommand ApproveClick { get; }
        public ICommand UnapproveClick { get; }

        private CommentDTO? commentDTO;
        public CommentDTO CommentDTO
        {
            get => commentDTO;
            set
            {
                if(commentDTO != value)
                {
                    commentDTO = value;
                    OnPropertyChanged(nameof(CommentDTO));
                }
            }
        }
        
        private IEnumerable<dynamic>? requests;
        public IEnumerable<dynamic> Requests
        {
            get { return requests; }
            set
            {
                if(requests != value)
                {
                    requests = value;
                    OnPropertyChanged(nameof(Requests));
                }
            }
        }

        public void GetRequestsDataGrid()
        {

            var dataObjects = from accommodation in accommodationService.GetAllDTO()
                              join image in imageService.BindImageToAccommodation() on accommodation.Id equals image.AccommodationId
                              join reservation in accommodationReservationService.GetAllDTO() on accommodation.Id equals reservation.AccommodationId
                              join moveRequest in accommodationReservationMoveRequestService.GetAllDTO() on reservation.Id equals moveRequest.ReservationId
                              join guest in userService.GetAll() on reservation.GuestId equals guest.Id
                              where accommodation.OwnerId == user.Id
                              where moveRequest.Status == 0
                              select new
                              {
                                  Accommodation = accommodation,
                                  Image = image,
                                  Reservation = reservation,
                                  MoveRequest = moveRequest,
                                  Guest = guest,
                                  BorderColorCondition = accommodationReservationService.CanMoveReservation( accommodation.Id, moveRequest.StartDate,moveRequest.EndDate)
                              };

            Requests = dataObjects;
        }

        private void AddComment()
        {
            MoveRequestCommentWindow moveRequestCommentWindow = new MoveRequestCommentWindow(CommentDTO);
            if (moveRequestCommentWindow.ShowDialog() == true)
            {

                CommentDTO.User = user.ToUser();
            }
        }

        private void ExecuteApproveClick(object parameter)
        {
            var selectedRow = parameter;

            if (selectedRow != null)
            {

                AccommodationReservationDTO selectedReservation = (AccommodationReservationDTO)(selectedRow.GetType().GetProperty("Reservation")?.GetValue(selectedRow, null));
                AccommodationReservationMoveRequestDTO selectedRequest = (AccommodationReservationMoveRequestDTO)(selectedRow.GetType().GetProperty("MoveRequest")?.GetValue(selectedRow, null));
                if (selectedReservation != null && selectedRequest != null)
                {
                    selectedReservation.StartDate = selectedRequest.StartDate;
                    selectedReservation.EndDate = selectedRequest.EndDate;
                    selectedRequest.Status = (ReservationMoveRequestStatus)1;

                    accommodationReservationService.Update(selectedReservation.ToReservation());
                    accommodationReservationMoveRequestService.Update(selectedRequest.ToRequest());
                }
            }
        }

        private void ExecuteUnapproveClick(object parameter)
        {
            var selectedRow = parameter;


            if (selectedRow != null)
            {
                AccommodationReservationMoveRequestDTO selectedRequest = (AccommodationReservationMoveRequestDTO)(selectedRow.GetType().GetProperty("MoveRequest")?.GetValue(selectedRow, null));
                if (selectedRequest != null)
                {
                    
                    selectedRequest.Status = (ReservationMoveRequestStatus)2;
                    AddComment();

                    selectedRequest.CommentId = commentService.Save(CommentDTO.ToComment()).Id;
                    accommodationReservationMoveRequestService.Update(selectedRequest.ToRequest());
                    
                }
            }
        }
        
        public ReservationsManagementViewModel(UserDTO user)
        {
            this.user = user;
            CommentDTO = new();

            GetRequestsDataGrid();

            ApproveClick = new RelayCommand(ExecuteApproveClick);
            UnapproveClick = new RelayCommand(ExecuteUnapproveClick);
        }
    }
}

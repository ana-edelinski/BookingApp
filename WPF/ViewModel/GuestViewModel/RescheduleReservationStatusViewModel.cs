using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model;
using BookingApp.WPF.DTOs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModel.GuestViewModel
{
    public class RescheduleReservationStatusViewModel : INotifyPropertyChanged
    {
        private readonly AccommodationReservationMoveRequestService _accommodationReservationMoveRequestService;
        private readonly CommentService _commentService = new();
        private readonly ImageService _imageService = new();
        private readonly AccommodationService _accommodationService = new();
        private readonly AccommodationReservationService _accommodationReservationService = new();
        public AccommodationReservationDTO SelectedReservation { get; set; }
        public CommentDTO Comment { get; set; }
        public AccommodationReservationMoveRequestDTO SelectedReservationMoveRequest { get; set; }
        public ObservableCollection<AccommodationReservationMoveRequestDTO> PresentableRequests { get; set; }
        public ICommand DownloadReportCommand { get; }


        public RescheduleReservationStatusViewModel(UserDTO loggedInUser, AccommodationReservationDTO selectedReservation)
        {
            SelectedReservation = selectedReservation;

            _accommodationReservationMoveRequestService = new AccommodationReservationMoveRequestService();
            SelectedReservationMoveRequest = new AccommodationReservationMoveRequestDTO();
            Comment = new CommentDTO();

            LoadReservationMoveRequests();
            DownloadReportCommand = new RelayCommand(ExecuteDownloadReportCommand);

        }

        private void ExecuteDownloadReportCommand(object parameter)
        {
            GenerateReservationMoveRequestsPDF.GenerateDocument(PresentableRequests);
        }

        private void LoadReservationMoveRequests()
        {
            PresentableRequests = new ObservableCollection<AccommodationReservationMoveRequestDTO>(
                _accommodationReservationMoveRequestService
                    .GetAll()
                    .Select(r => {
                        var comment = _commentService.GetCommentById(r.CommentId);
                        var accommodationId = _accommodationReservationService.GetAccommodationIdByReservationId(r.ReservationId);
                        var imagePath = GetImageForAccommodation(accommodationId);
                        return new AccommodationReservationMoveRequestDTO
                        {
                            Id = r.Id,
                            StartDate = r.StartDate,
                            EndDate = r.EndDate,
                            Status = r.Status,
                            CommentText = comment != null ? comment.Text : string.Empty,
                            ImagePath = imagePath,
                            AccommodationName = _accommodationReservationService.GetAccommodationNameForAccommodationReservation(r.ReservationId)

                        };
                    })
            );
        }


        private string GetImageForAccommodation(int accommodationId)   
        {
            var image = BindImageToReservation().FirstOrDefault(i => i.AccommodationId == accommodationId); 
            return image?.Path;
        }

        private List<Image> BindImageToReservation()
        {
            List<Image> images = new();
            foreach (AccommodationReservationDTO reservation in _accommodationReservationService.GetAllDTO())  
            {
                images.Add(_imageService.GetOneForAccommodation(reservation.AccommodationId));  
            }
            return images;  
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

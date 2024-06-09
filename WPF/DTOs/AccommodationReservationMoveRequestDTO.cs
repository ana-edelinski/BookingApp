using BookingApp.Domain.Model;
using BookingApp.Domain.Model.Enums;
using System;
using System.ComponentModel;

namespace BookingApp.WPF.DTOs
{
    public class AccommodationReservationMoveRequestDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }
        private int reservationId;
        public int ReservationId
        {
            get { return reservationId; }
            set
            {
                if (reservationId != value)
                {
                    reservationId = value;
                    OnPropertyChanged(nameof(ReservationId));
                }
            }
        }

        private DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                if(startDate != value)
                {
                    startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }

        private DateTime endDate;
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                if(endDate != value)
                {
                    endDate = value;
                    OnPropertyChanged(nameof(EndDate)); 
                }
            }
        }


        private ReservationMoveRequestStatus status;
        public ReservationMoveRequestStatus Status
        {
            get => status;
            set
            {
                if(status != value)
                {
                    status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        private int commentId;
        public int CommentId
        {
            get { return commentId; }
            set
            {
                if(commentId != value)
                {
                    commentId = value;
                    OnPropertyChanged(nameof(CommentId));
                }
            }
        }

        private int accommodationId;
        public int AccommodationId
        {
            get { return accommodationId; }
            set
            {
                if (accommodationId != value)
                {
                    accommodationId = value;
                    OnPropertyChanged(nameof(AccommodationId));
                }
            }
        }


        private string commentText;
        public string CommentText
        {
            get { return commentText; }
            set
            {
                if (commentText != value)
                {
                    commentText = value;
                    OnPropertyChanged(nameof(CommentText));
                }
            }
        }

        public string ImagePath { get; set; }
        public string AccommodationName { get; set; }

        public AccommodationReservationMoveRequestDTO()
        {

        }

        public AccommodationReservationMoveRequestDTO(int id, int reservationId, DateTime startDate, DateTime endDate, ReservationMoveRequestStatus status, int commentId)
        {
            Id = id;
            ReservationId = reservationId;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
            CommentId = commentId;
        }

        public AccommodationReservationMoveRequest ToRequest()
        {
            return new AccommodationReservationMoveRequest(Id,reservationId,startDate,endDate,status,commentId);
        }

        public AccommodationReservationMoveRequestDTO(AccommodationReservationMoveRequest a)
        {
            Id = a.Id;
            ReservationId = a.ReservationId;
            StartDate = a.StartDate;    
            EndDate = a.EndDate;
            CommentId = a.CommentId;
            Status = a.Status;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }
}

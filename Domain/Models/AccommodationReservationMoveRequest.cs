using System;
using System.ComponentModel.Design;
using BookingApp.Applications.UtilityInterfaces;
using BookingApp.Domain.Model.Enums;
using BookingApp.View.OwnerView;

namespace BookingApp.Domain.Model
{
    public class AccommodationReservationMoveRequest : ISerializable
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ReservationMoveRequestStatus Status { get; set; } // PENDING, APPROVED, UNAPPROVED
        public int CommentId { get; set; } //Need because owner can leave a comment when unapproving

        public AccommodationReservationMoveRequest()
        {
            Status = 0;
            CommentId = -1;
        }

        public AccommodationReservationMoveRequest(int id, int reservationId, DateTime startDate, DateTime endDate, ReservationMoveRequestStatus status, int commentId)
        {
            Id = id;
            ReservationId = reservationId;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
            CommentId = commentId;
        }

        public void FromCSV(string[] csvValues)
        {
            Id = int.Parse(csvValues[0]);
            ReservationId = int.Parse(csvValues[1]);
            StartDate = DateTime.Parse(csvValues[2]);
            EndDate = DateTime.Parse(csvValues[3]);
            Status = (ReservationMoveRequestStatus)Convert.ToInt32(csvValues[4]);
            CommentId = int.Parse(csvValues[5]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                ReservationId.ToString(),
                StartDate.ToString(),
                EndDate.ToString(),
                Convert.ToInt32(Status).ToString(),
                CommentId.ToString()
            };

            return csvValues;
        }
    }
}

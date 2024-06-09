using BookingApp.Applications.Utilities.Validation;
using BookingApp.Domain.Model;
using BookingApp.View.OwnerView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.DTOs
{
    public class GuestRatingDTO : ValidationBase
    {
        public int Id { get; set; }
        private int accommodationReservationId;
        public int AccommodationReservationId
        {
            get { return accommodationReservationId; }
            set
            {
                if (accommodationReservationId != value)
                {
                    accommodationReservationId = value;
                    OnPropertyChanged(nameof(AccommodationReservationId));
                }
            }
        }

        private int cleanliness;
        public int Cleanliness
        {
            get { return cleanliness; }
            set
            {
                if (cleanliness != value)
                {
                    cleanliness = value;
                    OnPropertyChanged(nameof(Cleanliness));
                }
            }
        }

        private int ruleRespect;
        public int RuleRespect
        {
            get { return ruleRespect; }
            set
            {
                if (ruleRespect != value)
                {
                    ruleRespect = value;
                    OnPropertyChanged(nameof(RuleRespect));
                }
            }

        }

        private int commentId;
        public int CommentId
        {
            get { return commentId; }
            set
            {
                if (commentId != value)
                {
                    commentId = value;
                    OnPropertyChanged(nameof(CommentId));
                }
            }
        }

        private bool isRated;
        public bool IsRated
        {
            get { return isRated; }
            set
            {
                if (isRated != value)
                {
                    isRated = value;
                    OnPropertyChanged(nameof(IsRated));
                }
            }
        }

        public string OwnerName { get; set; } 
        public string CommentText { get; set; }
        public List<string> CleanlinessStars => Enumerable.Repeat("⭐", Cleanliness).ToList();
        public List<string> RuleRespectStars => Enumerable.Repeat("⭐", RuleRespect).ToList();


        protected override void ValidateSelf()
        {
            if(Cleanliness < 1)
            {
                this.ValidationErrors["Cleanliness"] = "Cleanliness cannot be empty.";
            }
            if (RuleRespect < 1)
            {
                this.ValidationErrors["RuleRespect"] = "Rule respect cannot be empty.";
            }
        }
        public GuestRating ToGuestRating()
        {
            return new GuestRating(Id, AccommodationReservationId, Cleanliness, RuleRespect, CommentId, IsRated);
        }
        public GuestRatingDTO(GuestRating guestRating)
        {
            Id = guestRating.Id;
            AccommodationReservationId = guestRating.AccommodationReservationId;
            Cleanliness = guestRating.Cleanliness;
            RuleRespect = guestRating.RuleRespect;
            CommentId = guestRating.CommentId;
            IsRated = guestRating.IsRated;
        }

        public GuestRatingDTO(GuestRating guestRating, string ownerName, string commentText) 
        {
            Id = guestRating.Id;
            AccommodationReservationId = guestRating.AccommodationReservationId;
            Cleanliness = guestRating.Cleanliness;
            RuleRespect = guestRating.RuleRespect;
            CommentId = guestRating.CommentId;
            IsRated = guestRating.IsRated;
            OwnerName = ownerName; 
            CommentText = commentText; 
        }

        public GuestRatingDTO()
        {

        }
    }
}

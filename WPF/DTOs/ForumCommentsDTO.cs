using BookingApp.Applications.Utilities;
using System.Collections.Generic;

namespace BookingApp.WPF.DTOs
{
    public class ForumCommentsDTO : NotifyPropertyChangedImplemented
    {

        public CommentDTO SelectedComment { get; set; }
        public ForumDTO SelectedForum { get; set; }

        private CommentDTO comment;
        public CommentDTO Comment
        {
            get { return comment; }
            set
            {
                if (comment != value)
                {
                    comment = value;
                    OnPropertyChanged(nameof(comment));
                }
            }
        }

        private IEnumerable<dynamic> commentsDataGrid;
        public IEnumerable<dynamic> CommentsDataGrid
        {
            get { return commentsDataGrid; }
            set
            {
                if (commentsDataGrid != value)
                {
                    commentsDataGrid = value;
                    OnPropertyChanged(nameof(commentsDataGrid));
                }
            }
        }

        private LocationDTO selectedLocation;
        public LocationDTO SelectedLocation
        {
            get { return selectedLocation; }
            set
            {
                if (selectedLocation != value)
                {
                    selectedLocation = value;
                    OnPropertyChanged(nameof(selectedLocation));
                }
            }
        }

        private bool hasAccommodation;
        public bool HasAccommodation
        {
            get { return hasAccommodation; }
            set
            {
                if (hasAccommodation != value)
                {
                    hasAccommodation = value;
                    OnPropertyChanged(nameof(HasAccommodation));
                }
            }
        }

        public ForumCommentsDTO()
        {
            Comment = new();
            SelectedLocation = new();
            SelectedForum = new();
        }
    }
}

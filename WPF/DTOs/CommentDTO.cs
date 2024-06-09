using BookingApp.Applications.Utilities.Validation;
using BookingApp.Domain.Model;
using System;
using System.ComponentModel;

namespace BookingApp.WPF.DTOs
{
    public class CommentDTO : ValidationBase
    {
        public int Id { get; set; }
        private DateTime creationTime;
        public DateTime CreationTime
        {
            get { return creationTime; }
            set
            {
                if (creationTime != value)
                {
                    creationTime = value;
                    OnPropertyChanged(nameof(CreationTime));
                }
            }
        }

        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                if (text != value)
                {
                    text = value;
                    OnPropertyChanged(nameof(Text));
                }
            }
        }

        private User user;
        public User User
        {
            get { return user; }
            set
            {
                if (user != value)
                {
                    user = value;
                    OnPropertyChanged(nameof(User));
                }
            }
        }

        private int forumId;
        public int ForumId
        {
            get { return forumId; }
            set
            {
                if(forumId != value)
                {
                    forumId = value;
                    OnPropertyChanged(nameof(ForumId));
                }
            }
        }

        private int reportNumber;
        public int ReportNumber
        {
            get { return reportNumber; }
            set
            {
                if(reportNumber != value)
                {
                    reportNumber = value;
                    OnPropertyChanged(nameof(ReportNumber));
                }
            }
        }

        public string Author { get; set; }
        private bool wasPresent;
        public bool WasPresent
        {
            get { return wasPresent; }
            set
            {
                if (wasPresent != value)
                {
                    wasPresent = value;
                    OnPropertyChanged(nameof(WasPresent));
                }
            }
        }

        public bool IsOwner { get; set; }
        protected override void ValidateSelf()
        {
            if(string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            {
                this.ValidationErrors["Text"] = "Text cannot be empty.";
            }
        }
        public Comment ToComment()
        {
            return new Comment(Id,CreationTime, Text, User,ForumId, ReportNumber);
        }
        public CommentDTO()
        {
            Id = -1;
            User = new();
            ReportNumber = 0;
            ForumId = -1;
            CreationTime = DateTime.Now;
        }

        public CommentDTO(Comment comment)
        {
            Id = comment.Id;
            Text = comment.Text;
            User = comment.User;
            CreationTime = comment.CreationTime;
            ForumId = comment.ForumId;
            ReportNumber = comment.ReportNumber;
        }

        public CommentDTO(DateTime creationTime, string text, User user, int forumId, int reportNumber)
        {
            CreationTime = creationTime;
            Text = text;
            User = user;
            ForumId = forumId;
            ReportNumber = reportNumber;
        }
    }
}

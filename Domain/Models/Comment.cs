using BookingApp.Applications.UtilityInterfaces;
using System;

namespace BookingApp.Domain.Model
{
    public class Comment : ISerializable
    {
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string Text { get; set; }
        public User User { get; set; }
        public int ForumId {  get; set; }
        public int ReportNumber { get; set; }

        public bool IsOwner { get; set; } 


        public Comment() {
        
        }

        public Comment(int id,DateTime creationTime, string text, User user, int forumId, int reportNumber)
        {
            CreationTime = creationTime;
            Text = text;
            User = user;
            ForumId = forumId;
            ReportNumber = reportNumber;
            Id = id;
        }

        public Comment(DateTime creationTime, string text, User user)
        {
            CreationTime = creationTime;
            Text = text;
            User = user;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { 
                Id.ToString(), 
                CreationTime.ToString(), 
                Text, 
                User.Id.ToString() ,
                ForumId.ToString(),
                ReportNumber.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            CreationTime = Convert.ToDateTime(values[1]);
            Text = values[2];
            User = new User() { Id = Convert.ToInt32(values[3]) };
            ForumId = Convert.ToInt32(values[4]);
            ReportNumber = Convert.ToInt32(values[5]);
        }
    }
}

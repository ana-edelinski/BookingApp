using BookingApp.Applications.UtilityInterfaces;
using System;

namespace BookingApp.Domain.Models
{
    public class Forum : ISerializable
    {
        public int Id { get; set; }
        public int LocationId { get; set; } //So we can know for which location is forum
        public int GuestId { get; set; } //So we can know who opened the forum
        public DateTime CreationTime { get; set; } //THe owner is using this atribute so he/she can show the notification which will last for approx. 1 day

        //Comment model has ForumId, and also comment has number of reports atribute 
        public bool IsOpened { get; set; }
        public bool IsVeryUseful { get; set; }
        public int GuestCommentsCount { get; set; }
        public int OwnerCommentsCount { get; set; }
        public Forum() 
        {

        }

        public Forum(int id, int locationId, int guestId, DateTime creationTime, bool isOpened, int guestCommentsCount, int ownerCommentsCount, bool isVeryUseful)
        {
            Id = id;
            LocationId = locationId;
            GuestId = guestId;
            CreationTime = creationTime;
            IsOpened = isOpened;
            IsVeryUseful = isVeryUseful;
            GuestCommentsCount = guestCommentsCount;
            OwnerCommentsCount = ownerCommentsCount;
        }

        public string[] ToCSV()
        {
            string[] csvValues = 
            {
                Id.ToString(),
                LocationId.ToString(),
                GuestId.ToString(),
                CreationTime.ToString(),
                IsOpened.ToString(),
                IsVeryUseful.ToString(), 
                GuestCommentsCount.ToString(), 
                OwnerCommentsCount.ToString()
            };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            LocationId = Convert.ToInt32(values[1]);
            GuestId = Convert.ToInt32(values[2]);
            CreationTime = Convert.ToDateTime(values[3]);
            IsOpened = Convert.ToBoolean(values[4]);
            IsVeryUseful = Convert.ToBoolean(values[5]);
            GuestCommentsCount = Convert.ToInt32(values[6]);
            OwnerCommentsCount = Convert.ToInt32(values[7]);
        }

        
    }
}

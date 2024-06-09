using System.ComponentModel;

namespace BookingApp.WPF.DTOs
{
    public class RenovationRecommendationDTO : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System.ComponentModel;

namespace BookingApp.Applications.Utilities
{
    public class NotifyPropertyChangedImplemented : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

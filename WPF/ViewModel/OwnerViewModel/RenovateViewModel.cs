using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model;
using BookingApp.View.OwnerView;
using BookingApp.WPF.DTOs;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.OwnerViewModel
{
    public class RenovateViewModel : NotifyPropertyChangedImplemented
    {
        private readonly RenovationService renovationService = new();
        private RenovationViewModelService renovationViewModelService;
        private readonly AccommodationReservationService accommodationReservationService = new();

        public ICommand RenovateClick { get; }
        public ICommand FindTermsClick { get; }

        private NavigationService Navigate { get; set; }
        public ICommand PreviousPageCommand { get; private set; }
        public ICommand RenovateCommand { get; private set; }

        private readonly int accommodationId;
        public bool HasLength;

        private ObservableCollection<DateSpan> foundTerms;
        public ObservableCollection<DateSpan> FoundTerms
        {
            get { return foundTerms; }
            set
            {
                if(value != foundTerms)
                {
                    foundTerms = value;
                    OnPropertyChanged(nameof(FoundTerms));
                }
            }
        }

        private DateSpan selectedDateSpan;
        public DateSpan SelectedDateSpan
        {
            get { return selectedDateSpan; }
            set
            {
                if(selectedDateSpan != value)
                {
                    selectedDateSpan = value;
                    OnPropertyChanged(nameof(SelectedDateSpan));
                }
            }
        }
        private DateTime startDateDP;
        public DateTime StartDateDP
        {
            get { return startDateDP; }
            set
            {
                if(startDateDP != value)
                {
                    startDateDP = value;
                    OnPropertyChanged(nameof(StartDateDP)); 
                }
            }
        }

        private DateTime endDateDP;
        public DateTime EndDateDP
        {
            get { return endDateDP; }
            set
            {
                if (endDateDP != value)
                {
                    endDateDP = value;
                    OnPropertyChanged(nameof(EndDateDP));
                }
            }
        }

        private int estimatedLength;
        public int EstimatedLength
        {
            get { return estimatedLength; }
            set
            {
                if(estimatedLength != value)
                {
                    estimatedLength = value;
                    HasLength = estimatedLength >= 1;
                    OnPropertyChanged(nameof(EstimatedLength));
                }
            }
        }

        private void ExecuteFindTermsClick(object parameter)
        {
            FoundTerms.Clear();
            renovationViewModelService = new(estimatedLength);
            if (StartDateDP > EndDateDP || StartDateDP < DateTime.Now) return;

            foreach(DateSpan dateSpan in renovationViewModelService.FindDateRanges(accommodationReservationService.GetAvailableDates(accommodationId, StartDateDP, EndDateDP)))
            {
                FoundTerms.Add(dateSpan);
            }
            
        }

        private void ExecuteRenovateClick(object parameter)
        {
            renovationService.Save(new RenovationDTO(accommodationId,SelectedDateSpan.StartDate,SelectedDateSpan.EndDate).ToRenovation());
            Navigate.Navigate(new Accommodations(User, Navigate));
        }

        private void ExecutePreviousPage(object parameter)
        {
            Navigate.GoBack();
        }

        private UserDTO User { get; set; }
        public RenovateViewModel(int accommodationId, NavigationService navigationService, UserDTO user) 
        {
            this.accommodationId = accommodationId;
            Navigate = navigationService;
            User = user;
            FoundTerms = new();
            StartDateDP = DateTime.Now;
            EndDateDP = DateTime.Now;

            FindTermsClick = new RelayCommand(ExecuteFindTermsClick);
            RenovateCommand = new RelayCommand(ExecuteRenovateClick);
            PreviousPageCommand = new RelayCommand(ExecutePreviousPage);
        }
    }
}

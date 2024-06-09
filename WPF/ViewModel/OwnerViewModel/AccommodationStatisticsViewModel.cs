using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.WPF.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Previewer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace BookingApp.WPF.ViewModel.OwnerViewModel
{

    public class AccommodationStatisticsViewModel : NotifyPropertyChangedImplemented
    {
        private readonly AccommodationReservationStatisticsService accommodationReservationStatisticsService = new();
        private readonly AccommodationReservationMoveRequestService accommodationReservationMoveRequest = new();
        private readonly RenovationRecommendationService renovationRecommendationService = new();

        public ICommand PrintCommand { get; private set; }

        private ObservableCollection<string> comboBoxOptions;
        public ObservableCollection<string> ComboBoxOptions
        {
            get { return comboBoxOptions; }
            set
            {
                if (comboBoxOptions != value)
                {
                    comboBoxOptions = value;
                    OnPropertyChanged(nameof(ComboBoxOptions));
                }
            }
        }

        private string selectedOption;
        public string SelectedOption
        {
            get { return selectedOption; }
            set 
            {
                if (selectedOption != value)
                {
                    selectedOption = value;
                    SetColumnHeaders();
                    OnPropertyChanged(nameof(SelectedOption));
                    GetStatisticsDataGrid();
                }
            } 
        }

        private ObservableCollection<string> columnHeaders;
        public ObservableCollection<string> ColumnHeaders
        {
            get { return columnHeaders; }
            set
            {
                if(columnHeaders != value)
                {
                    columnHeaders = value;
                    OnPropertyChanged(nameof(ColumnHeaders));
                }
            }
        }

        private AccommodationDTO accommodation;
        public AccommodationDTO Accommodation
        {
            get { return accommodation; }
            set
            {
                if (accommodation != value)
                {
                    accommodation = value;
                    OnPropertyChanged(nameof(Accommodation));   
                }
            }
        }

        private IEnumerable<dynamic> statistics;
        public IEnumerable<dynamic> Statistics
        {
            get { return statistics; }
            set
            {
                if(statistics != value)
                {
                    statistics = value;
                    OnPropertyChanged(nameof(Statistics));
                }
            }
        }

        private string theBestTime;
        public string TheBestTime
        {
            get { return theBestTime; }
            set
            {
                if(theBestTime != value)
                {
                    theBestTime = value;
                    OnPropertyChanged(nameof(TheBestTime));
                }
            }
        }
        private void SetColumnHeaders()
        {
            if(SelectedOption == "All years")
            {
                ColumnHeaders = new ObservableCollection<string> { "Year", "Reservations", "Cancelations", "Reschedulings", "Renovation Recommendations", "Busyness(%)" };
            }
            else
            {
                ColumnHeaders = new ObservableCollection<string> { "Month", "Reservations", "Cancelations", "Reschedulings", "Renovation Recommendations", "Busyness(%)" };
            }
        }

        private string GetTheBestTime(IEnumerable<dynamic> data)
        {
            
            dynamic theBestTime = null;
            theBestTime = data.FirstOrDefault();

            foreach(dynamic item in data)
            {
                if(theBestTime.Busyness < item.Busyness)
                {
                    theBestTime = item;
                }
            }
            if (theBestTime == null || theBestTime.Busyness == 0) return "-";
            return $"{theBestTime.Time} ({theBestTime.Busyness}%)";
        }

        private void GetStatisticsDataGrid()
        {
            Statistics = new List<dynamic>();
            if (SelectedOption == "All years")
            {
                foreach(int year in accommodationReservationStatisticsService.GetAllYears(accommodation.Id))
                {
                    int reservations = accommodationReservationStatisticsService.CountByYear(accommodation.Id,year, false);
                    int cancelations = accommodationReservationStatisticsService.CountByYear(accommodation.Id, year, true);
                    int reschedulings = accommodationReservationMoveRequest.CountByYear(accommodation.Id, year);
                    int recommendations = renovationRecommendationService.CountByYear(accommodation.Id,year);
                    int busyness = accommodationReservationStatisticsService.GetOccupancyByYear(accommodation.Id, year);

                    Statistics = Statistics.Concat(new[]
                                                 { new { Reservations = reservations,
                                                         Cancelations = cancelations,
                                                         Reschedulings = reschedulings,
                                                         Recommendations = recommendations,
                                                         Busyness = busyness,
                                                         Time = year.ToString()}});
                }

                    
            }
            else
            {
                for (int month = 1; month <= 12; month++)
                {
                    int reservations = accommodationReservationStatisticsService.CountByMonth(accommodation.Id, month, Convert.ToInt32(selectedOption), false);
                    int cancelations = accommodationReservationStatisticsService.CountByMonth(accommodation.Id, month, Convert.ToInt32(selectedOption), true);
                    int reschedulings = accommodationReservationMoveRequest.CountByMonth(accommodation.Id, month, Convert.ToInt32(selectedOption));
                    int recommendations = renovationRecommendationService.CountByMonth(accommodation.Id,month, Convert.ToInt32(selectedOption));
                    DateTime time = new(2000, month, 1); //2000 year, 1 day are random numbers because we just need a month name
                    int busyness = accommodationReservationStatisticsService.GetOccupancyByMonth(accommodation.Id,month, Convert.ToInt32(selectedOption));

                    Statistics = Statistics.Concat(new[]
                                                 { new { Reservations = reservations,
                                                         Cancelations = cancelations,
                                                         Reschedulings = reschedulings,
                                                         Recommendations = recommendations,
                                                         Busyness = busyness,
                                                         Time = time.ToString("MMMM")}});
                }
            }

            TheBestTime = GetTheBestTime(Statistics);
        }

        private void ExecutePrintCommand(object parameter)
        {
            GenerateStatisticsPDF.GenerateDocument(Statistics,SelectedOption, Accommodation.Name);
        }
        
        public AccommodationStatisticsViewModel(AccommodationDTO selectedAccommodation) 
        {
            accommodation = selectedAccommodation;
            ComboBoxOptions = new ObservableCollection<string> { "All years", "2024", "2023", "2022", "2021", "2020" };

            SelectedOption = ComboBoxOptions.FirstOrDefault();

            PrintCommand = new RelayCommand(ExecutePrintCommand);
        }
    }
}

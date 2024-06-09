using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BookingApp.Repository;
using Image = BookingApp.Domain.Model.Image;
using System.Collections.ObjectModel;
using Xceed.Wpf.Toolkit;
using System.Diagnostics;
using System.ComponentModel;
using BookingApp.Domain.Model;
using BookingApp.Domain.Model.Enums;
using BookingApp.Applications.UseCases;
using BookingApp.WPF.DTOs;
using BookingApp.View.OwnerView;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for AccommodationViewSearch.xaml
    /// </summary>
    public partial class AccommodationViewSearch : Page
    {
        
        private readonly LocationService _locationService;
        private readonly AccommodationService _accommodationService;
        private readonly AccommodationReservationService _accommodationReservationService;
        private readonly ImageService _imageService;

        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public List<string> Countries { get; set; }
        public List<string> Cities { get; set; }
        public string SelectedCountry { get; set; }
        public string SelectedCity { get; set; }

        private readonly UserDTO user;
        private List<AccommodationType> selectedTypes = new List<AccommodationType>();
        private Accommodation _selectedAccommodation;

        public AccommodationViewSearch(UserDTO user)
        {
            InitializeComponent();
            this.DataContext = this;
            this.user = user;

            _locationService = new LocationService();
            _accommodationService = new AccommodationService();
            _imageService = new ImageService();

            Accommodations = new ObservableCollection<Accommodation>(_accommodationService.GetAll());

            Countries = _locationService.GetAllCountries();
            Countries.Insert(0, "Country");
            SelectedCountry = Countries[0];

            GetAccommodationsDataGrid();
        }

        private void NumberUpDownValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var upDown = (sender as IntegerUpDown);
            if (upDown.Value == 0)
            {
                upDown.Text = "";
            }

        }


        private void CountrySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedCountry != "")
            {
                Cities = _locationService.GetCitiesByCountry(SelectedCountry);
                Cities.Insert(0, "City");
                cityComboBox.ItemsSource = Cities;
                cityComboBox.SelectedItem = 0;
                SelectedCity = Cities[0];
                cityComboBox.Text = Cities[0];
            }

        }

        private void CheckBox_CheckedOrUnchecked(object sender, RoutedEventArgs e)
        {
            var checkBox = (sender as CheckBox);
            if (checkBox != null)
            {
                AccommodationType type;
                switch (checkBox.Content)
                {
                    case "APARTMENT":
                        type = AccommodationType.APARTMENT;
                        break;
                    case "HOUSE":
                        type = AccommodationType.HOUSE;
                        break;
                    case "COTTAGE":
                        type = AccommodationType.COTTAGE;
                        break;
                    default:
                        return;
                }

                if (checkBox.IsChecked == true)
                {
                    selectedTypes.Add(type);
                }
                else
                {
                    selectedTypes.Remove(type);
                }

                typeComboBox.Text = string.Join(", ", selectedTypes.Select(t => t.ToString()));

            }
        }
        private void Search(object sender, RoutedEventArgs e)
        {
            var filtered = Accommodations
                .Where(accommodation =>
                    IsNameMatch(accommodation) &&
                    IsGuestNumberMatch(accommodation) &&
                    IsDayNumberMatch(accommodation) &&
                    IsLocationMatch(accommodation) &&
                    IsTypeMatch(accommodation))
                .Join(BindImageToAccommodation(),
                    accommodation => accommodation.Id,
                    image => image.AccommodationId,
                    (accommodation, image) => new { Accommodation = accommodation, Image = image })
                .ToList();

            presentableAccommodations.ItemsSource = filtered;
            presentableAccommodations.Items.Refresh();
        }


        private bool IsNameMatch(Accommodation accommodation)
        {
            return string.IsNullOrEmpty(nameTextBox.Text) || accommodation.Name.ToLower().Contains(nameTextBox.Text.ToLower());
        }

        private bool IsGuestNumberMatch(Accommodation accommodation)
        {
            if (int.TryParse(guestsNumberTextBox.Text, out int guests))
            {
                return guests == 0 || guests <= accommodation.Capacity;
            }
            return true; 
        }

        private bool IsDayNumberMatch(Accommodation accommodation)
        {
            if (int.TryParse(daysNumberTextBox.Text, out int days))
            {
                return days == 0 || days >= accommodation.MinReservationDays;
            }
            return true; 
        }

        private bool IsLocationMatch(Accommodation accommodation)
        {
            return (countryComboBox.SelectedIndex == 0 && cityComboBox.SelectedIndex == 0) ||
                   (cityComboBox.SelectedIndex == 0 && accommodation.Location.Country.ToLower().Contains(countryComboBox.Text.ToLower())) ||
                   (accommodation.Location.Country.ToLower().Contains(countryComboBox.Text.ToLower()) &&
                    accommodation.Location.City.ToLower().Contains(cityComboBox.Text.ToLower()));
        }

        private bool IsTypeMatch(Accommodation accommodation)
        {
            return selectedTypes.Count == 0 || selectedTypes.Contains(accommodation.Type);
        }




        private void CancelSearch(object sender, RoutedEventArgs e)
        {
            var joinedData = from accommodation in Accommodations
                             join image in BindImageToAccommodation() on accommodation.Id equals image.AccommodationId
                             select new { Accommodation = accommodation, Image = image };

            presentableAccommodations.ItemsSource = joinedData;
            presentableAccommodations.Items.Refresh();

            nameTextBox.Text = "";
            countryComboBox.SelectedItem = 0;
            SelectedCountry = Countries[0];
            countryComboBox.Text = Countries[0];
            cityComboBox.SelectedItem = 0;
            SelectedCity = Cities[0];
            cityComboBox.Text = Cities[0];
            foreach (var item in typeComboBox.Items)
            {
                if (item is CheckBox checkBox)
                {
                    checkBox.IsChecked = false;
                }
            }
            guestsNumberTextBox.Text = "";
            daysNumberTextBox.Text = "";
        }


        private void PageSwitch(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var accommodation = button.DataContext as dynamic;
            presentableAccommodations.SelectedItem = accommodation;
            MakeReservation(sender, e);
        }

  
        private void MakeReservation(object sender, RoutedEventArgs e)
        {
            if (presentableAccommodations.SelectedItem != null)
            {
                SelectedAccommodation = (presentableAccommodations.SelectedItem as dynamic).Accommodation;

                AccommodationReservationPage accommodationReservationPage = new AccommodationReservationPage(SelectedAccommodation, user, _accommodationService, _accommodationReservationService);
                NavigationService?.Navigate(accommodationReservationPage);
            }
        }


        private void GetAccommodationsDataGrid()
        {
            var joinedData = from accommodation in _accommodationService.GetAll()
                             join image in BindImageToAccommodation() on accommodation.Id equals image.AccommodationId
                             select new { Accommodation = accommodation, Image = image };
            presentableAccommodations.ItemsSource = joinedData;
        }

        private List<Image> BindImageToAccommodation()
        {
            List<Image> images = new();
            foreach (Accommodation accommodation in _accommodationService.GetAll())
            {
                images.Add(_imageService.GetOneForAccommodation(accommodation.Id));
            }
            return images;
        }


    }
}
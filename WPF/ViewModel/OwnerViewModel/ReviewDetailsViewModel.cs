using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model;
using BookingApp.View.OwnerView;
using BookingApp.WPF.DTOs;
using System;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.OwnerViewModel
{
    public class ReviewDetailsViewModel : NotifyPropertyChangedImplemented
    {
        private readonly CommentService commentService = new();
        public ReviewDetailsDTO DataViewModel { get; private set; }

        private int currentIndex = 0;
        public ICommand PreviousImageClick { get; private set; }
        public ICommand NextImageClick { get; private set; }
        public ICommand GoBackCommand { get; private set; }
        private NavigationService Navigate {  get; set; }
        private UserDTO User {  get; set; }
        private dynamic selectedItem;
        public dynamic SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                    SetData();
                    OnPropertyChanged(nameof(SelectedItem));
                }
            }
        }
        private void SetData()
        {
            DataViewModel = new();
            if (SelectedItem != null)
            {
                DataViewModel.Accommodation = (AccommodationDTO)(SelectedItem.GetType().GetProperty("Accommodation")?.GetValue(SelectedItem, null));
                DataViewModel.Guest = (UserDTO)(SelectedItem.GetType().GetProperty("User")?.GetValue(SelectedItem, null));
                DataViewModel.Reservation = (AccommodationReservationDTO)(SelectedItem.GetType().GetProperty("AccommodationReservation")?.GetValue(SelectedItem, null));
                DataViewModel.Rating = (OwnerAccommodationRatingDTO)(SelectedItem.GetType().GetProperty("OwnerAccommodationRating")?.GetValue(SelectedItem, null));
                DataViewModel.Comment = commentService.GetById(DataViewModel.Rating.CommentId);
            }
            DisplayImages();
        }
        private void ExecutePreviousImageClick(object paremeter)
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                DisplayImages();
            }
        }

        private void ExecuteNextImageClick(object paremeter)
        {
            if (DataViewModel.ImagesDTO.Count > 3 && currentIndex < DataViewModel.ImagesDTO.Count - 3)
            {
                currentIndex++;
                DisplayImages();
            }
        }

        private void DisplayImages()
        {
            DataViewModel.Images.Clear();

            int start = currentIndex;

            for (int i = start; i < start + 3 && i < DataViewModel.ImagesDTO.Count; i++)
            {
                var image = new System.Windows.Controls.Image
                {
                    Source = new BitmapImage(new Uri(DataViewModel.ImagesDTO[i].Path, UriKind.RelativeOrAbsolute)),
                    Width = 150,
                    Height = 150
                };
                DataViewModel.Images.Add(image);
            }
        }

        private void ExecuteGoBackCommand(object paremeter)
        {
            Navigate.GoBack();   
        }
        public ReviewDetailsViewModel(dynamic selectedReview, NavigationService navigationService)
        {
            SelectedItem = selectedReview;
            Navigate = navigationService;
            PreviousImageClick = new RelayCommand(ExecutePreviousImageClick);
            NextImageClick = new RelayCommand(ExecuteNextImageClick);
            GoBackCommand = new RelayCommand(ExecuteGoBackCommand);
        }
    }
}

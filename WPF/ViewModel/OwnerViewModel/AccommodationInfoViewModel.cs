using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model;
using BookingApp.View.OwnerView;
using BookingApp.WPF.DTOs;
using BookingApp.WPF.View.OwnerView;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.OwnerViewModel
{
    public class AccommodationInfoViewModel : NotifyPropertyChangedImplemented
    {
        private readonly LocationService locationService = new();
        private readonly ImageService imageService = new(); 

        private NavigationService Navigate { get; set; }
        public ICommand NextImageClick { get; }
        public ICommand PreviousImageClick { get; }
        public ICommand NavigateToPageCommand { get; private set; }

        private int currentIndex = 0;
        private UserDTO User { get; set; }
        private AccommodationDTO accommodationDTO;
        public AccommodationDTO AccommodationDTO
        {
            get { return accommodationDTO; }
            set
            {
                if (value != accommodationDTO)
                {
                    accommodationDTO = value;
                    OnPropertyChanged(nameof(AccommodationDTO));
                }
            }
        }

        private ObservableCollection<System.Windows.Controls.Image> images;
        public ObservableCollection<System.Windows.Controls.Image> Images
        {
            get { return images; }
            set
            {
                if (images != value)
                {
                    images = value;

                    OnPropertyChanged(nameof(Images));
                }
            }
        }
        private ObservableCollection<ImageDTO> imagesDTO;
        public ObservableCollection<ImageDTO> ImagesDTO
        {
            get { return imagesDTO; }
            set
            {
                if (imagesDTO != value)
                {
                    imagesDTO = value;
                    OnPropertyChanged(nameof(ImagesDTO));
                }
            }
        }

        private void ExecutePreviousImageClick(object paremeter)
        {
            if (currentIndex >= 0)
            {
                currentIndex--;
                DisplayImages();
            }
        }

        private void ExecuteNextImageClick(object paremeter)
        {
            if (imagesDTO.Count > 3 && currentIndex < imagesDTO.Count - 2)
            {
                currentIndex++;
                DisplayImages();
            }
        }

        private void DisplayImages()
        {
            Images.Clear();

            int start = currentIndex;
            int idx = 0;
            for (int i = start; i < start + 3 && i < imagesDTO.Count; i++)
            {
                var image = new System.Windows.Controls.Image();
                if (i < 0)
                {
                    i+=1;
                    idx++;
                }
                image.Source = new BitmapImage(new Uri(imagesDTO[i].Path, UriKind.RelativeOrAbsolute));
                if(idx == 1)
                {
                    image.Width = 210;
                    image.Height = 230;
                }
                else
                {
                    image.Width = 80;
                    image.Height = 90;
                }
                
                idx+=1;
                Images.Add(image);
            }
        }

        private void ExecuteNavigateToPage(object paremeter)
        {
            switch (paremeter)
            {
                case "Renovate":
                    Navigate.Navigate(new Renovate(User, AccommodationDTO.Id, Navigate));
                    break;
                case "Statistics":
                    Navigate.Navigate(new AccommodationStatistics(AccommodationDTO));
                    break;

            }
        }
        public AccommodationInfoViewModel(NavigationService navigationService, AccommodationDTO selectedAccommodation, UserDTO user) 
        {
            User = user;

            Navigate = navigationService;

            AccommodationDTO = new();
            AccommodationDTO = selectedAccommodation;
            AccommodationDTO.Location = locationService.GetById(AccommodationDTO.LocationId);

            Images = new();
            ImagesDTO = imageService.GetAllForAccommodation(AccommodationDTO.Id);

            DisplayImages();
            NextImageClick = new RelayCommand(ExecuteNextImageClick);
            PreviousImageClick = new RelayCommand(ExecutePreviousImageClick);
            NavigateToPageCommand = new RelayCommand(ExecuteNavigateToPage);
        }
    }
}

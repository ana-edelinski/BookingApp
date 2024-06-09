using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.View.OwnerView;
using BookingApp.WPF.DTOs;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.OwnerViewModel
{
    public class AccommodationRegistrationViewModel3 : NotifyPropertyChangedImplemented
    {
        private readonly AccommodationService accommodationService = new();
        private readonly ImageService imageService = new();

        public ICommand RegisterClick { get; private set; }
        public ICommand AddImageClick { get; private set; }
        public ICommand NextImageClick { get; private set; }
        public ICommand PreviousImageClick { get; private set; }
        public ICommand PreviousPageCommand { get; private set; }

        private int currentIndex = 0;

        private NavigationService Navigate {  get; set; }
        private UserDTO User { get; set; }

        private AccommodationDTO accommodationDTO;
        public AccommodationDTO AccommodationDTO
        {
            get { return accommodationDTO; }
            set
            {
                if (accommodationDTO != value)
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
        private void DisplayImages()
        {
            Images.Clear();

            int start = currentIndex;

            for (int i = start; i < start + 3 && i < imagesDTO.Count; i++)
            {
                var image = new System.Windows.Controls.Image();
                image.Source = new BitmapImage(new Uri(ImagesDTO[i].OriginPath));
                image.Width = 150;
                image.Height = 150;
                Images.Add(image);
            }
        }
        private string imagesMessage;
        public string ImagesMessage
        {
            get { return imagesMessage; }
            set
            {
                if(imagesMessage != value)
                {
                    imagesMessage = value;
                    OnPropertyChanged(nameof(ImagesMessage));
                }
            }
        }

        private bool ValidateImages()
        {
            if (ImagesDTO.Count < 1)
            {
                ImagesMessage = "At least one image.";
                return false;
            }
            else
            {
                ImagesMessage = "";
                return true;
            }
        }
        private void ExecuteRegisterClick(object paremeter)
        {
            AccommodationDTO.Validate();
            if (AccommodationDTO.IsValid && ValidateImages())
            {
                AccommodationDTO = new AccommodationDTO(accommodationService.Save(AccommodationDTO.ToAccommodation()));

                for (int i = 0; i < ImagesDTO.Count; i++)
                {
                    ImagesDTO[i].AccommodationId = AccommodationDTO.Id;
                    imageService.Save(ImagesDTO[i].ToImage());
                    File.Copy(ImagesDTO[i].OriginPath, ImagesDTO[i].Path, true);
                }

                Navigate.Navigate(new Accommodations(User, Navigate));
            }
            
        }


        private void ExecuteAddImageClick(object parameter)
        {
            ImageDTO imageDTO = new();
            string selectedImagePath;
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.gif; *.bmp)|*.jpg; *.jpeg; *.png; *.gif; *.bmp|All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = @"E:\Downloads";
            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                selectedImagePath = openFileDialog.FileName;

                string imageName = Path.GetFileName(selectedImagePath);
                string destinationPath = "../../../Resources/AccommodationImages/" + imageName;
                imageDTO.Path = destinationPath;
                imageDTO.OriginPath = selectedImagePath;
                ImagesDTO.Add(imageDTO);
                DisplayImages();
            }
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
            if (imagesDTO.Count > 3 && currentIndex < imagesDTO.Count - 3)
            {
                currentIndex++;
                DisplayImages();
            }
        }

        private void ExecutePreviousPageCommand(object parameter)
        {
            Navigate.Navigate(new AccommodationRegistrationP2(AccommodationDTO,User, ImagesDTO, Navigate));
        }
        public AccommodationRegistrationViewModel3(AccommodationDTO accommodationDTO, UserDTO user, ObservableCollection<ImageDTO> images, NavigationService navigationService)
        {
            AccommodationDTO = accommodationDTO;
            AccommodationDTO.CurrentPage = 3;
            User = user;
            ImagesDTO = images;
            Navigate = navigationService;

            Images = new();
            DisplayImages();

            RegisterClick = new RelayCommand(ExecuteRegisterClick);
            AddImageClick = new RelayCommand(ExecuteAddImageClick);
            PreviousImageClick = new RelayCommand(ExecutePreviousImageClick);
            NextImageClick = new RelayCommand(ExecuteNextImageClick);
            PreviousPageCommand = new RelayCommand(ExecutePreviousPageCommand);
        }
    }
}

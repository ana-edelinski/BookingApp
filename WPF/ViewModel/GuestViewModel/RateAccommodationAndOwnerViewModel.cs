using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model;
using BookingApp.WPF.DTOs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModel.GuestViewModel
{
    public class RateAccommodationAndOwnerViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private readonly OwnerAccommodationRatingService _ownerAccommodationRatingService = new();
        private readonly AccommodationReservationService _accommodationReservationService;
        private readonly AccommodationService _accommodationService = new();
        private readonly ImageService _imageService;
        private readonly CommentService _commentService;

        private readonly User user = new();
        private List<string> filePaths = new();
        public AccommodationReservationDTO Reservation { get; set; }

        public RelayCommand AddPictureCommand { get; set; }
        public RelayCommand RateCommand { get; set; }

        public int Id { get; set; }

        private ObservableCollection<string> _imageUrls = new ObservableCollection<string>();
        public ObservableCollection<string> ImageUrls
        {
            get => _imageUrls;
            set
            {
                if (_imageUrls != value)
                {
                    _imageUrls = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _cleanliness;
        public int Cleanliness
        {
            get => _cleanliness;
            set
            {
                if (value != _cleanliness)
                {
                    _cleanliness = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _correctness;
        public int Correctness
        {
            get => _correctness;
            set
            {
                if (value != _correctness)
                {
                    _correctness = value;
                    OnPropertyChanged();
                }
            }
        }

        private CommentDTO _additionalComment;
        public CommentDTO AdditionalComment
        {
            get => _additionalComment;
            set
            {
                if (value != _additionalComment)
                {
                    _additionalComment = value;
                    OnPropertyChanged();
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

        private OwnerAccommodationRatingDTO ownerAccommodationRatingDTO;
        public OwnerAccommodationRatingDTO OwnerAccommodationRatingDTO
        {
            get { return ownerAccommodationRatingDTO; }
            set
            {
                if(ownerAccommodationRatingDTO != value)
                {
                    ownerAccommodationRatingDTO = value;
                    OnPropertyChanged(nameof(OwnerAccommodationRatingDTO));
                }
            }
        }
        private CommentDTO commentDTO;
        public CommentDTO CommentDTO
        {
            get { return commentDTO; }
            set
            {
                if (commentDTO != value)
                {
                    commentDTO = value;
                    OnPropertyChanged(nameof(CommentDTO));
                }
            }
        }


        private OwnerAccommodationRatingDTO _selectedUnratedAccommodation;
        public OwnerAccommodationRatingDTO SelectedUnratedAccommodation
        {
            get => _selectedUnratedAccommodation;
            set
            {
                if (value != _selectedUnratedAccommodation)
                {
                    _selectedUnratedAccommodation = value;
                    OnPropertyChanged(nameof(SelectedUnratedAccommodation));
                    OnPropertyChanged(nameof(AccommodationName));
                    OnPropertyChanged(nameof(OwnerName));
                }
            }
        }

        private bool _cleanliness1IsChecked;
        public bool Cleanliness1IsChecked
        {
            get => _cleanliness1IsChecked;
            set
            {
                if (value != _cleanliness1IsChecked)
                {
                    _cleanliness1IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _cleanliness2IsChecked;
        public bool Cleanliness2IsChecked
        {
            get => _cleanliness2IsChecked;
            set
            {
                if (value != _cleanliness2IsChecked)
                {
                    _cleanliness2IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _cleanliness3IsChecked;
        public bool Cleanliness3IsChecked
        {
            get => _cleanliness3IsChecked;
            set
            {
                if (value != _cleanliness3IsChecked)
                {
                    _cleanliness3IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _cleanliness4IsChecked;
        public bool Cleanliness4IsChecked
        {
            get => _cleanliness4IsChecked;
            set
            {
                if (value != _cleanliness4IsChecked)
                {
                    _cleanliness4IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _cleanliness5IsChecked;
        public bool Cleanliness5IsChecked
        {
            get => _cleanliness5IsChecked;
            set
            {
                if (value != _cleanliness5IsChecked)
                {
                    _cleanliness5IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _correctness1IsChecked;
        public bool Correctness1IsChecked
        {
            get => _correctness1IsChecked;
            set
            {
                if (value != _correctness1IsChecked)
                {
                    _correctness1IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _correctness2IsChecked;
        public bool Correctness2IsChecked
        {
            get => _correctness2IsChecked;
            set
            {
                if (value != _correctness2IsChecked)
                {
                    _correctness2IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _correctness3IsChecked;
        public bool Correctness3IsChecked
        {
            get => _correctness3IsChecked;
            set
            {
                if (value != _correctness3IsChecked)
                {
                    _correctness3IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _correctness4IsChecked;
        public bool Correctness4IsChecked
        {
            get => _correctness4IsChecked;
            set
            {
                if (value != _correctness4IsChecked)
                {
                    _correctness4IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _correctness5IsChecked;
        public bool Correctness5IsChecked
        {
            get => _correctness5IsChecked;
            set
            {
                if (value != _correctness5IsChecked)
                {
                    _correctness5IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        public string AccommodationName
        {
            get => SelectedUnratedAccommodation?.AccommodationName;
        }
        public string OwnerName
        {
            get => SelectedUnratedAccommodation?.UserName;
        }

        public RateAccommodationAndOwnerViewModel(RateRecommendViewModel rateReccommentViewModel)
        {
            _accommodationReservationService = new AccommodationReservationService();
            _commentService = new CommentService();
            _imageService = new ImageService();

            AdditionalComment = new CommentDTO();

            SelectedUnratedAccommodation = rateReccommentViewModel.SelectedUnratedAccommodation;
            Reservation = _accommodationReservationService.GetByIdDTO(SelectedUnratedAccommodation.AccommodationReservationId);

            ImagesDTO = new();
            AccommodationDTO = new();
            OwnerAccommodationRatingDTO = new();
            CommentDTO = new();

            AddPictureCommand = new RelayCommand(Execute_AddPictureCommand, CanExecute_Command);
            RateCommand = new RelayCommand(Execute_RateCommand, CanExecute_Command);
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            CommandManager.InvalidateRequerySuggested();
        }

        private void SetRatingsForCleanlinees()
        {
            if (Cleanliness1IsChecked == true)
            {
                Cleanliness = 1;
            }
            else if (Cleanliness2IsChecked == true)
            {
                Cleanliness = 2;
            }
            else if (Cleanliness3IsChecked == true)
            {
                Cleanliness = 3;
            }
            else if (Cleanliness4IsChecked == true)
            {
                Cleanliness = 4;
            }
            else if (Cleanliness5IsChecked == true)
            {
                Cleanliness = 5;
            }
        }

        private void SetRatingsForCorrectness()
        {
            if (Correctness1IsChecked == true)
            {
                Correctness = 1;
            }
            else if (Correctness2IsChecked == true)
            {
                Correctness = 2;
            }
            else if (Correctness3IsChecked == true)
            {
                Correctness = 3;
            }
            else if (Correctness4IsChecked == true)
            {
                Correctness = 4;
            }
            else if (Correctness5IsChecked == true)
            {
                Correctness = 5;
            }
        }

        private bool CanExecute_Command(object obj)
        {
            return IsValid;
        }
      
        private void Execute_AddPictureCommand(object obj)
        {
            ImageDTO imageDTO = new();
            string selectedImagePath;
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.gif; *.bmp)|*.jpg; *.jpeg; *.png; *.gif; *.bmp|All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = @"D:\sims";
            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                selectedImagePath = openFileDialog.FileName;

                string imageName = Path.GetFileName(selectedImagePath);
                string destinationPath = "../../../Resources/AccommodationRatingsImages/" + imageName;
                imageDTO.Path = destinationPath;
                filePaths.Add(selectedImagePath);
                ImagesDTO.Add(imageDTO);
                for (int i = 0; i < ImagesDTO.Count; i++)
                {
                    ImagesDTO[i].AccommodationId = AccommodationDTO.Id;
                    ImagesDTO[i].OwnerAccommodationRatingId = OwnerAccommodationRatingDTO.Id;
                    _imageService.Save(ImagesDTO[i].ToImage());
                    File.Copy(filePaths[i], ImagesDTO[i].Path, true);
                }
                ImageUrls.Add(destinationPath);
            }
        }


        private void Execute_RateCommand(object obj)
        {
            if (IsValid)
            {
                var messageBoxResult = MessageBox.Show($"Would you like to save your rating?", "Rating Accommodation Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    SetRatingsForCleanlinees();
                    SetRatingsForCorrectness();

                    CommentDTO newComment = new CommentDTO(DateTime.Now, AdditionalComment.Text, user, 0, 0);
                    _commentService.Save(newComment.ToComment());

                    OwnerAccommodationRatingDTO ownerAccommodationRatingDTO = new OwnerAccommodationRatingDTO
                    {
                        Id = Id,
                        AccommodationReservationId = Reservation.Id,
                        OwnerRating = Correctness,
                        AccommodationRating = Cleanliness,
                        CommentId = newComment.Id,
                        IsRated = true
                    };

                    _ownerAccommodationRatingService.Save(ownerAccommodationRatingDTO.ToOwnerAccommodationRating());
                    MessageBox.Show("Rating saved successfully.");
                }
                return;
            }
        }


        public string Error => null;
        public string this[string columnName]
        {
            get
            {
                string error = null;

                if (columnName == "Comment" && string.IsNullOrEmpty(AdditionalComment.Text))
                {
                    error = "Comment is required!";
                }
                if (columnName == "Cleanliness" && !Cleanliness1IsChecked && !Cleanliness2IsChecked && !Cleanliness3IsChecked && !Cleanliness4IsChecked && !Cleanliness5IsChecked)
                {
                    error = "Must be selected one option for cleanliness!";
                }
                if (columnName == "Correctness" && !Correctness1IsChecked && !Correctness2IsChecked && !Correctness3IsChecked && !Correctness4IsChecked && !Correctness5IsChecked)
                {
                    error = "Must be selected one option for correctness!";
                }
                return error;
            }

        }

        private readonly string[] _validatedProperties = { "Comment", "Cleanliness", "Correctness" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }

    }
}
using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.WPF.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModel.OwnerViewModel
{
    public class RenovationsViewModel : NotifyPropertyChangedImplemented
    {
        private readonly AccommodationService accommodationService = new();
        private readonly ImageService imageService = new();
        private readonly RenovationService renovationService = new();
        public ICommand CancelClick { get; }

        private IEnumerable<dynamic> renovations;
        public IEnumerable<dynamic> Renovations
        {
            get { return renovations; }
            set
            {
                if (renovations != value)
                {
                    renovations = value;
                    OnPropertyChanged(nameof(Renovations));
                }
            }
        }

        private void GetRenovationsDataGrid(UserDTO user)
        {
            var dataObjects = from accommodation in accommodationService.GetAllForOwner(user.Id)
                              join image in imageService.BindImageToAccommodation() on accommodation.Id equals image.AccommodationId
                              join renovation in renovationService.GetAllDTO() on accommodation.Id equals renovation.AccommodationId
                              select new
                              {
                                  Accommodation = accommodation,
                                  Image = image,
                                  Renovation = renovation,
                                  IsDisabled = renovationService.IsLessThan5Days(renovation)
                              };

            Renovations = dataObjects;
        }

        


        private void ExecuteCancelClick(object parameter)
        {
            object selectedRow = parameter ;

            if(selectedRow != null)
            {
                RenovationDTO selectedRenovation = (RenovationDTO)(selectedRow.GetType().GetProperty("Renovation")?.GetValue(selectedRow, null));
                if(selectedRenovation != null)
                {
                    renovationService.Delete(selectedRenovation.ToRenovation());
                }
            }
        }
        public RenovationsViewModel(UserDTO user)
        {

            GetRenovationsDataGrid(user);
            CancelClick = new RelayCommand(ExecuteCancelClick);
        }
    }
}

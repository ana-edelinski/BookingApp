using BookingApp.Applications.UseCases;
using BookingApp.Applications.Utilities;
using BookingApp.WPF.DTOs;
using System.ComponentModel;

namespace BookingApp.WPF.ViewModel.OwnerViewModel
{
    public class AccountInfoViewModel : NotifyPropertyChangedImplemented
    {
        private readonly OwnerAccommodationRatingService ownerAccommodationRatingService = new();       private UserDTO ownerDTO;
        public UserDTO OwnerDTO
        {
            get { return ownerDTO; }
            set
            {
                if(ownerDTO != value)
                {
                    ownerDTO = value;
                    OnPropertyChanged(nameof(OwnerDTO));    
                }
            }
        }

        public string IsSuperOwner
        {
            get { return SetSuperOwner(); }
        }

        private string SetSuperOwner()
        {
            return ownerAccommodationRatingService.IsSuperOwner(OwnerDTO.Id) ? "Super-Owner" : "Owner";
        }

        public AccountInfoViewModel(UserDTO user)
        {
            OwnerDTO = user;

        }
    }
}

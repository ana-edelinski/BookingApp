using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repository;
using BookingApp.View.OwnerView;
using BookingApp.WPF.DTOs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BookingApp.Applications.UseCases 
{
    public class ImageService : IImageRepository
    {
        private readonly IImageRepository _repository;
        private readonly AccommodationService accommodationService = new();

        public ImageService()
        {
            _repository = Injector.CreateInstance<IImageRepository>();
        }
        public Image Delete(Image image)
        {
            return _repository.Delete(image);
        }

        public List<Image> GetAll()
        {
            return _repository.GetAll();
        }

        public Image GetById(int id)
        {
            return _repository.GetById(id);
        }

        public Image Save(Image image)
        {
            return _repository.Save(image);
        }

        public Image? GetOneForAccommodation(int accommodationId)
        {
            return GetAll().FirstOrDefault(i => i.AccommodationId == accommodationId);
        }

        public ImageDTO? GetOneForAccommodationDTO(int accommodationId)
        {
            Image image = GetAll().FirstOrDefault(i => i.AccommodationId == accommodationId);
            return image != null ? new ImageDTO(image) : null;
        }


        public ObservableCollection<ImageDTO> GetAllForAccommodation(int accommodationId)
        {
            ObservableCollection<ImageDTO> images = new(); 
              
            foreach(Image image in GetAll().FindAll(image => image.AccommodationId == accommodationId))
            {
                images.Add(new ImageDTO(image));
            }

            return images;
        }

        public List<Image> BindImageToAccommodation() 
        {
            List<Image> images = new();
            foreach (Accommodation accommodation in accommodationService.GetAll())
            {
                images.Add(GetOneForAccommodation(accommodation.Id));
            }
            return images;
        }

        public ObservableCollection<ImageDTO> GetAllForReview(int reviewId)
        {
            ObservableCollection<ImageDTO> images = new();

            foreach (Image image in GetAll().FindAll(image => image.OwnerAccommodationRatingId == reviewId))
            {
                images.Add(new ImageDTO(image));
            }

            return images;
        }
    }
}

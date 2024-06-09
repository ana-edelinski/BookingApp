using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model;
using BookingApp.Observer;
using BookingApp.Domain.RepositoryInterfaces;
using System;

namespace BookingApp.Repository
{
    public class ImageRepository : IImageRepository
    {
        private const string filePath = "../../../Resources/Data/images.csv";
        private readonly Serializer<Image> _serializer;

        private List<Image> _images;

        public ImageRepository() 
        {
            _serializer = new Serializer<Image>();
            _images = new List<Image>();

        }

        public Image Save(Image image)
        {
            image.Id = NextId();
            _images = _serializer.FromCSV(filePath);
            _images.Add(image);
            _serializer.ToCSV(filePath, _images);

            return image;
        }

        public int NextId()
        {
            _images = _serializer.FromCSV(filePath);
            if(_images.Count < 1)
            {
                return 0;
            }
            return _images.Max(i => i.Id) + 1;
        }

        public Image Delete(Image image)
        {
            _images = _serializer.FromCSV(filePath);
            Image found = _images.Find(c => c.Id == image.Id);
            _images.Remove(found);
            _serializer.ToCSV(filePath, _images);
            return image;
        }
        public Image? GetById(int id)
        {
            _images = _serializer.FromCSV(filePath);
            return _images.Find(i => i.Id == id);
        }

        public List<Image> GetAll()
        {
            return _serializer.FromCSV(filePath);
        }

        //remove below
        public Image? GetOneForAccommodation(int accommodationId)
        {
            return _serializer.FromCSV(filePath).FirstOrDefault(i => i.AccommodationId == accommodationId);
        }

        public List<Image> GetAllForAccommodation(int accommodationId)
        {
            return _serializer.FromCSV(filePath).FindAll(image => image.AccommodationId == accommodationId);
        }

    }
}

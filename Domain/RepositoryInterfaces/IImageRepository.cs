using BookingApp.Domain.Model;
using System.Collections.Generic;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IImageRepository
    {
        public Image Save(Image image);
        public Image Delete(Image image);
        public Image GetById(int id);
        public List<Image> GetAll();
    }
}

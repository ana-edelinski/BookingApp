using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.View.OwnerView;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Documents;

namespace BookingApp.Repositories
{
    public class RenovationRepository : IRenovationRepository
    {
        private const string filePath = "../../../Resources/Data/renovations.csv";
        private readonly Serializer<Renovation> _serializer;

        private List<Renovation> _renovations;

        public RenovationRepository()
        {
            _serializer = new Serializer<Renovation>();
            _renovations = _serializer.FromCSV(filePath);
        }

        public Renovation Save(Renovation renovation)
        {
            renovation.Id = NextId();
            _renovations = _serializer.FromCSV(filePath);
            _renovations.Add(renovation);
            _serializer.ToCSV(filePath, _renovations);

            return renovation;
        }

        private int NextId()
        {
            _renovations = _serializer.FromCSV(filePath);
            if (_renovations.Count < 1)
            {
                return 0;
            }
            return _renovations.Max(a => a.Id) + 1;
        }

        public void Delete(Renovation renovation)
        {
            _renovations = _serializer.FromCSV(filePath);
            Renovation? found = _renovations.Find(a => a.Id == renovation.Id);
            _renovations.Remove(found);
            _serializer.ToCSV(filePath, _renovations);
        }

        public List<Renovation> GetAll()
        {
            return _renovations;
        }
    }
}

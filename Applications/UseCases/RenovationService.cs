using BookingApp.Applications.Utilities;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Applications.UseCases
{
    internal class RenovationService : IRenovationRepository
    {
        private readonly IRenovationRepository _renovationRepository;

        public RenovationService()
        {
            _renovationRepository = Injector.CreateInstance<IRenovationRepository>();
        }

        public void Delete(Renovation renovation)
        {
            _renovationRepository.Delete(renovation);
        }

        public List<Renovation> GetAll()
        {
            return _renovationRepository.GetAll();
        }

        public Renovation Save(Renovation renovation)
        {
            return _renovationRepository.Save(renovation);
        }

        public List<RenovationDTO> GetAllDTO()
        {
            List<RenovationDTO> renovations = new();
            foreach(Renovation renovation in GetAll())
            {
                renovations.Add(new RenovationDTO(renovation));
            }

            return renovations;
        }

        public List<Renovation> GetAllForAccommodation(int accommodationId)
        {
            return GetAll().FindAll(renovation => renovation.AccommodationId == accommodationId);
        }

        public List<DateTime> GetTakenDates(int accommodationId)
        {
            List<DateTime> takenDates = new();
            
            foreach(Renovation renovation in GetAllForAccommodation(accommodationId))
            {
                for(DateTime date = renovation.StartDate; date <= renovation.EndDate; date = date.AddDays(1))
                {
                    takenDates.Add(date);
                }
            }

            return takenDates;
        }

        public bool IsLessThan5Days(RenovationDTO renovation)
        {
            return !((renovation.StartDate - DateTime.Now).TotalDays >= 5);
        }
    }
}

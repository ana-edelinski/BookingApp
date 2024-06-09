using BookingApp.Applications.Utilities;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Applications.UseCases
{
    public class RenovationRecommendationService : IRenovationRecommendationRepository
    {
        private IRenovationRecommendationRepository _repository;

        public RenovationRecommendationService()
        {
            _repository = Injector.CreateInstance<IRenovationRecommendationRepository>();
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<RenovationRecommendation> GetAll()
        {
            return _repository.GetAll();
        }

        public int NextId()
        {
            return _repository.NextId();
        }

        public RenovationRecommendation Save(RenovationRecommendation renovationRecommendation)
        {
            return _repository.Save(renovationRecommendation);
        }

        public RenovationRecommendation Update(RenovationRecommendation renovationRecommendation)
        {
            throw new NotImplementedException();
        }

        public int CountByYear(int accommodationId, int year)
        {
            int count = 0;
            
            foreach(RenovationRecommendation renovationRecommendation in GetAll().FindAll(rr => rr.AccommodationId == accommodationId))
            {
                if (renovationRecommendation.CreationTime.Year == year) count++;
            }

            return count;
        }

        public int CountByMonth(int accommodationId, int month, int year) 
        {
            int count = 0;

            foreach (RenovationRecommendation renovationRecommendation in GetAll().FindAll(rr => rr.AccommodationId == accommodationId))
            {
                if (renovationRecommendation.CreationTime.Year == year && renovationRecommendation.CreationTime.Month == month) count++;
            }

            return count;
        }
    }
}

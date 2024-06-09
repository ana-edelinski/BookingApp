using BookingApp.Domain.Models;
using System.Collections.Generic;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IRenovationRecommendationRepository
    {
        public RenovationRecommendation Save(RenovationRecommendation renovationRecommendation);
        public RenovationRecommendation Update(RenovationRecommendation renovationRecommendation);
        public void Delete(int id);
        public List<RenovationRecommendation> GetAll();
        public int NextId();
    }
}

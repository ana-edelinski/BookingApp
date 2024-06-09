using BookingApp.Applications.Utilities;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repositories
{
    public class RenovationRecommendationRepository : IRenovationRecommendationRepository
    {
        private static string filePath = "../../../Resources/Data/renovationRecommendations.csv";

        private Serializer<RenovationRecommendation> serializer;
        private List<RenovationRecommendation> recommendations;
        public RenovationRecommendationRepository() 
        {
            serializer = new Serializer<RenovationRecommendation>();
            recommendations = serializer.FromCSV(filePath);
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<RenovationRecommendation> GetAll()
        {
            return recommendations;
        }

        public int NextId()
        {
            recommendations = serializer.FromCSV(filePath);
            if (recommendations.Count < 1)
            {
                return 1;
            }
            return recommendations.Max(c => c.Id) + 1;
        }

        public RenovationRecommendation Save(RenovationRecommendation renovationRecommendations)
        {
            renovationRecommendations.Id = NextId();

            recommendations = serializer.FromCSV(filePath);
            recommendations.Add(renovationRecommendations);
            serializer.ToCSV(filePath, recommendations);

            return renovationRecommendations;
        }

        public RenovationRecommendation Update(RenovationRecommendation renovationRecommendation)
        {
            throw new NotImplementedException();
        }
    }
}

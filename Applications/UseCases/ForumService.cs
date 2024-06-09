using BookingApp.Applications.Utilities;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Applications.UseCases
{
    public class ForumService : IForumRepository
    {
        private readonly IForumRepository forumRepository;
       
        public ForumService() 
        {
            forumRepository = Injector.CreateInstance<IForumRepository>();
        }
        public List<Forum> GetAll()
        {
            return forumRepository.GetAll();
        }

        public List<ForumDTO> GetAllDTO()
        {
            List<ForumDTO> forums = new();
            foreach(Forum forum in GetAll())
            {
                forums.Add(new ForumDTO(forum));
            }

            return forums;
        }

        public Forum GetById(int id)
        {
            return forumRepository.GetById(id);
        }

        public ForumDTO GetByIdDTO(int id)
        {
            var forum = GetById(id);
            return new ForumDTO().FromForum(forum);
        }

        public Forum Save(Forum forum)
        {
            return forumRepository.Save(forum);
        }
        
        public Forum Update(Forum forum)
        {
            return forumRepository.Update(forum);
        }

        public ForumDTO Update(ForumDTO forumDTO)
        {
            var forum = forumDTO.ToForum();
            var updatedForum = Update(forum);
            return new ForumDTO().FromForum(updatedForum);
        }

        public bool ForumExists(int locationId)
        {
            return forumRepository.GetAll().Any(f => f.LocationId == locationId);
        }

    }
}

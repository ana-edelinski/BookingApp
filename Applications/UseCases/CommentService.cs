using BookingApp.Applications.Utilities;
using BookingApp.Domain.Model;
using BookingApp.Domain.Model.Enums;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repository;
using BookingApp.WPF.DTOs;
using BookingApp.WPF.View.OwnerView;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace BookingApp.Applications.UseCases
{
    public class CommentService : ICommentRepository
    {
        private readonly ICommentRepository _repository;
        public CommentService()
        {
            _repository = Injector.CreateInstance<ICommentRepository>();   
        }
        public void Delete(Comment comment)
        {
            _repository.Delete(comment);
        }

        public List<Comment> GetAll()
        {
            return _repository.GetAll();
        }

        public List<CommentDTO> GetAllDTO()
        {
            List<CommentDTO> comments = new();

            foreach(Comment comment in _repository.GetAll())
            {
                comments.Add(new CommentDTO(comment));
            }

            return comments;
        }

        public List<Comment> GetByUser(User user)
        {
            return _repository.GetByUser(user);
        }

        public Comment Save(Comment comment)
        {
            return _repository.Save(comment);
        }

        public Comment Update(Comment comment)
        {
            return _repository.Update(comment);
        }

        public CommentDTO GetById(int id)
        {
            return new CommentDTO(GetAll().Find(c => c.Id == id));
        }

        public List<CommentDTO> GetAllForForum(int forumId)
        {
            List<CommentDTO> comments = new();
            foreach (Comment comment in GetAll().FindAll(comment => comment.ForumId == forumId))
            {
                comments.Add(new CommentDTO(comment));
            }

            return comments;
        }

        public bool IsForumUseful(int forumId)
        {
            UserService userService = new();
            int guestCount = 0;
            int ownerCount = 0;

            foreach(CommentDTO comment in GetAllForForum(forumId))
            {
                if (userService.GetById(comment.User.Id).UserType == (UserType)1)
                {
                    guestCount++;
                }
                else if(userService.GetById(comment.User.Id).UserType == 0)
                {
                    ownerCount++;
                }
            }

            return CompareCommentCount(guestCount,ownerCount);
        }

        private bool CompareCommentCount(int guestCount,int ownerCount)
        {
            return guestCount >= 20 && ownerCount >= 10;
        }

        public Comment GetCommentById(int id)
        {
            return _repository.GetCommentById(id);
        }


        public CommentDTO IncrementReportNumber(CommentDTO comment)
        {
            comment.ReportNumber++;
            return comment;
        }

        public string GetCommentTextForGuestRating(int commentId)
        {
            var comment = _repository.GetCommentById(commentId);
            return comment?.Text ?? ""; 
        }

        public List<CommentDTO> GetCommentsByForumId(int forumId)
        {
            List<CommentDTO> comments = new();
            foreach (Comment comment in GetAll().FindAll(c => c.ForumId == forumId))
            {
                comments.Add(new CommentDTO(comment));
            }
            return comments;
        }

        public string GetCommentAuthorName(int commentId)
        {
            var comment = _repository.GetCommentById(commentId);
            if (comment != null)
            {
                var userService = new UserService();
                var user = userService.GetById(comment.User.Id);    

                return user?.Username ?? "Unknown";
            }

            return "Unknown";
        }

        public Comment GetFirstCommentByForumId(int forumId)
        {
            return _repository.GetAll().Where(c => c.ForumId == forumId).OrderBy(c => c.CreationTime).FirstOrDefault();
        }


    }
}

using BookingApp.Applications.Utilities;
using BookingApp.WPF.DTOs;
using System.ComponentModel;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModel.OwnerViewModel
{
    public class MoveRequestCommentViewModel : NotifyPropertyChangedImplemented
    {
        public ICommand CommentClick { get; }
        private CommentDTO commentDTO;
        public CommentDTO CommentDTO
        {
             get { return commentDTO; }
            set
            {
                if(commentDTO != value)
                {
                    commentDTO = value;
                    OnPropertyChanged(nameof(CommentDTO));
                }
            }
        }
        private void ExecuteCommentClick(object parameter)
        {
            CommentDTO.CreationTime = System.DateTime.Now;
        }

        public MoveRequestCommentViewModel(CommentDTO comment)
        {
            CommentDTO = comment;
            CommentClick = new RelayCommand(ExecuteCommentClick);
        }
    }
}

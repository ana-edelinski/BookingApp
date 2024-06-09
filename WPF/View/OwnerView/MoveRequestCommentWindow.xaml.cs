using BookingApp.Domain.Model;
using BookingApp.WPF.DTOs;
using BookingApp.WPF.ViewModel.OwnerViewModel;
using System.Windows;

namespace BookingApp.View.OwnerView
{

    public partial class MoveRequestCommentWindow : Window
    {
        public MoveRequestCommentWindow(CommentDTO comment)
        {
            InitializeComponent();
            DataContext = new MoveRequestCommentViewModel(comment);
        }

        private void CommentButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

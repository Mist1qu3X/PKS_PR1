using System;
using System.Linq;
using System.Windows;
using LibraryApp.Data;
using LibraryApp.Models;

namespace LibraryApp
{
    public partial class AuthorsWindow : Window
    {
        private LibraryContext _context;

        public AuthorsWindow(LibraryContext context)
        {
            InitializeComponent();
            _context = context;
            LoadAuthors();
        }

        private void LoadAuthors()
        {
            AuthorsGrid.ItemsSource = _context.Authors.ToList();
        }

        private void AddAuthor_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AuthorWindow(_context);
            if (dialog.ShowDialog() == true)
                LoadAuthors();
        }

        private void EditAuthor_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorsGrid.SelectedItem is Author author)
            {
                var dialog = new AuthorWindow(_context, author.Id);
                if (dialog.ShowDialog() == true)
                    LoadAuthors();
            }
            else
            {
                MessageBox.Show("Выберите автора");
            }
        }

        private void DeleteAuthor_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorsGrid.SelectedItem is Author author)
            {
                // Проверяем, есть ли у автора книги
                if (_context.Books.Any(b => b.AuthorId == author.Id))
                {
                    MessageBox.Show("Нельзя удалить автора, у которого есть книги");
                    return;
                }

                if (MessageBox.Show($"Удалить автора {author.FirstName} {author.LastName}?", 
                    "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _context.Authors.Remove(author);
                    _context.SaveChanges();
                    LoadAuthors();
                }
            }
            else
            {
                MessageBox.Show("Выберите автора");
            }
        }
    }
}
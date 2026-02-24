using System.Linq;
using System.Windows;
using LibraryApp.Data;
using LibraryApp.Models;

namespace LibraryApp
{
    public partial class GenresWindow : Window
    {
        private LibraryContext _context;

        public GenresWindow(LibraryContext context)
        {
            InitializeComponent();
            _context = context;
            LoadGenres();
        }

        private void LoadGenres()
        {
            GenresGrid.ItemsSource = _context.Genres.ToList();
        }

        private void AddGenre_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new GenreWindow(_context);
            if (dialog.ShowDialog() == true)
                LoadGenres();
        }

        private void EditGenre_Click(object sender, RoutedEventArgs e)
        {
            if (GenresGrid.SelectedItem is Genre genre)
            {
                var dialog = new GenreWindow(_context, genre.Id);
                if (dialog.ShowDialog() == true)
                    LoadGenres();
            }
            else
            {
                MessageBox.Show("Выберите жанр");
            }
        }

        private void DeleteGenre_Click(object sender, RoutedEventArgs e)
        {
            if (GenresGrid.SelectedItem is Genre genre)
            {
                // Проверяем, есть ли книги с этим жанром
                if (_context.Books.Any(b => b.GenreId == genre.Id))
                {
                    MessageBox.Show("Нельзя удалить жанр, который используется в книгах");
                    return;
                }

                if (MessageBox.Show($"Удалить жанр '{genre.Name}'?", 
                    "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _context.Genres.Remove(genre);
                    _context.SaveChanges();
                    LoadGenres();
                }
            }
            else
            {
                MessageBox.Show("Выберите жанр");
            }
        }
    }
}
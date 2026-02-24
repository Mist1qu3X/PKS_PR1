using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Data;
using LibraryApp.Models;

namespace LibraryApp
{
    public partial class MainWindow : Window
    {
        private LibraryContext db;

        public MainWindow()
        {
            InitializeComponent();
            db = new LibraryContext();
            
            // Загружаем данные
            LoadFilters();
            LoadBooks();
            
            // Подключаем события
            SearchBox.TextChanged += SearchBox_TextChanged;
            AuthorFilter.SelectionChanged += Filter_Changed;
            GenreFilter.SelectionChanged += Filter_Changed;
            ResetButton.Click += ResetFilters_Click;
            AddButton.Click += AddBook_Click;
            EditButton.Click += EditBook_Click;
            DeleteButton.Click += DeleteBook_Click;
            AuthorsButton.Click += ManageAuthors_Click;
            GenresButton.Click += ManageGenres_Click;
        }

        private void LoadFilters()
        {
            var authors = db.Authors.ToList();
            authors.Insert(0, new Author { Id = 0, FirstName = "Все", LastName = "" });
            AuthorFilter.ItemsSource = authors;
            AuthorFilter.DisplayMemberPath = "FullName";
            AuthorFilter.SelectedValuePath = "Id";
            AuthorFilter.SelectedIndex = 0;

            var genres = db.Genres.ToList();
            genres.Insert(0, new Genre { Id = 0, Name = "Все жанры" });
            GenreFilter.ItemsSource = genres;
            GenreFilter.DisplayMemberPath = "Name";
            GenreFilter.SelectedValuePath = "Id";
            GenreFilter.SelectedIndex = 0;
        }

        private void LoadBooks()
        {
            var query = db.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .AsQueryable();

            if (AuthorFilter.SelectedValue != null && (int)AuthorFilter.SelectedValue > 0)
                query = query.Where(b => b.AuthorId == (int)AuthorFilter.SelectedValue);

            if (GenreFilter.SelectedValue != null && (int)GenreFilter.SelectedValue > 0)
                query = query.Where(b => b.GenreId == (int)GenreFilter.SelectedValue);

            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
                query = query.Where(b => b.Title.Contains(SearchBox.Text));

            var books = query.ToList();
            BooksGrid.ItemsSource = books;
            TotalBooksText.Text = $"Всего книг: {books.Sum(b => b.QuantityInStock)}";
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e) => LoadBooks();
        private void Filter_Changed(object sender, SelectionChangedEventArgs e) => LoadBooks();

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            AuthorFilter.SelectedIndex = 0;
            GenreFilter.SelectedIndex = 0;
            SearchBox.Text = "";
        }

        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new BookWindow(db);
            if (dialog.ShowDialog() == true)
                LoadBooks();
        }

        private void EditBook_Click(object sender, RoutedEventArgs e)
        {
            if (BooksGrid.SelectedItem is Book book)
            {
                var dialog = new BookWindow(db, book.Id);
                if (dialog.ShowDialog() == true)
                    LoadBooks();
            }
            else
                MessageBox.Show("Выберите книгу");
        }

        private void DeleteBook_Click(object sender, RoutedEventArgs e)
        {
            if (BooksGrid.SelectedItem is Book book)
            {
                if (MessageBox.Show($"Удалить '{book.Title}'?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    db.Books.Remove(book);
                    db.SaveChanges();
                    LoadBooks();
                }
            }
            else
                MessageBox.Show("Выберите книгу");
        }

        private void ManageAuthors_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AuthorsWindow(db);
            dialog.ShowDialog();
            LoadFilters();
            LoadBooks();
        }

        private void ManageGenres_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new GenresWindow(db);
            dialog.ShowDialog();
            LoadFilters();
            LoadBooks();
        }
    }
}
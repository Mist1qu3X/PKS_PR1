using System;
using System.Linq;
using System.Windows;
using LibraryApp.Data;
using LibraryApp.Models;

namespace LibraryApp
{
    public partial class BookWindow : Window
    {
        private LibraryContext _context;
        private Book? _currentBook;

        public BookWindow(LibraryContext context, int? bookId = null)
        {
            InitializeComponent();
            _context = context;

            // Загружаем авторов и жанры для выпадающих списков
            LoadComboBoxes();

            if (bookId.HasValue)
            {
                Title = "Редактирование книги";
                _currentBook = _context.Books.Find(bookId);
                LoadBookData();
            }
            else
            {
                Title = "Добавление книги";
            }
        }

        private void LoadComboBoxes()
        {
            AuthorBox.ItemsSource = _context.Authors.ToList();
            GenreBox.ItemsSource = _context.Genres.ToList();
        }

        private void LoadBookData()
        {
            if (_currentBook != null)
            {
                TitleBox.Text = _currentBook.Title;
                AuthorBox.SelectedValue = _currentBook.AuthorId;
                GenreBox.SelectedValue = _currentBook.GenreId;
                YearBox.Text = _currentBook.PublishYear.ToString();
                IsbnBox.Text = _currentBook.ISBN;
                QuantityBox.Text = _currentBook.QuantityInStock.ToString();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(TitleBox.Text))
            {
                MessageBox.Show("Введите название книги");
                return;
            }

            if (AuthorBox.SelectedValue == null)
            {
                MessageBox.Show("Выберите автора");
                return;
            }

            if (GenreBox.SelectedValue == null)
            {
                MessageBox.Show("Выберите жанр");
                return;
            }

            if (!int.TryParse(YearBox.Text, out int year) || year < 1000 || year > DateTime.Now.Year)
            {
                MessageBox.Show("Введите корректный год");
                return;
            }

            if (string.IsNullOrWhiteSpace(IsbnBox.Text))
            {
                MessageBox.Show("Введите ISBN");
                return;
            }

            if (!int.TryParse(QuantityBox.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Введите корректное количество");
                return;
            }

            try
            {
                if (_currentBook == null)
                {
                    // Добавление новой книги
                    _currentBook = new Book();
                    _context.Books.Add(_currentBook);
                }

                // Обновление полей
                _currentBook.Title = TitleBox.Text;
                _currentBook.AuthorId = (int)AuthorBox.SelectedValue;
                _currentBook.GenreId = (int)GenreBox.SelectedValue;
                _currentBook.PublishYear = year;
                _currentBook.ISBN = IsbnBox.Text;
                _currentBook.QuantityInStock = quantity;

                _context.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void AddAuthor_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AuthorWindow(_context);
            if (dialog.ShowDialog() == true && dialog.AddedOrEditedAuthorId.HasValue)
            {
                LoadComboBoxes();
                AuthorBox.SelectedValue = dialog.AddedOrEditedAuthorId.Value;
            }
        }

        private void AddGenre_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new GenreWindow(_context);
            if (dialog.ShowDialog() == true && dialog.AddedOrEditedGenreId.HasValue)
            {
                LoadComboBoxes();
                GenreBox.SelectedValue = dialog.AddedOrEditedGenreId.Value;
            }
        }
    }
}
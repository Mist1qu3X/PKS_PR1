using System;
using System.Windows;
using LibraryApp.Data;
using LibraryApp.Models;

namespace LibraryApp
{
    public partial class GenreWindow : Window
    {
        private LibraryContext _context;
        private int? _genreId;

        /// <summary>Id добавленного или отредактированного жанра (после успешного сохранения)</summary>
        public int? AddedOrEditedGenreId { get; private set; }

        public GenreWindow(LibraryContext context, int? genreId = null)
        {
            InitializeComponent();
            _context = context;
            _genreId = genreId;

            if (_genreId.HasValue)
            {
                Title = "Редактирование жанра";
                LoadGenreData();
            }
            else
            {
                Title = "Добавление жанра";
            }
        }

        private void LoadGenreData()
        {
            var genre = _context.Genres.Find(_genreId);
            if (genre != null)
            {
                NameBox.Text = genre.Name;
                DescriptionBox.Text = genre.Description;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("Введите название жанра");
                return;
            }

            try
            {
                Genre genre;
                if (_genreId.HasValue)
                {
                    genre = _context.Genres.Find(_genreId)!;
                    if (genre != null)
                    {
                        genre.Name = NameBox.Text;
                        genre.Description = DescriptionBox.Text;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    genre = new Genre
                    {
                        Name = NameBox.Text,
                        Description = DescriptionBox.Text
                    };
                    _context.Genres.Add(genre);
                }

                _context.SaveChanges();
                AddedOrEditedGenreId = genre.Id;
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
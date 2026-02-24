using System;
using System.Windows;
using LibraryApp.Data;
using LibraryApp.Models;

namespace LibraryApp
{
    public partial class AuthorWindow : Window
    {
        private LibraryContext _context;
        private int? _authorId;

        /// <summary>Id добавленного или отредактированного автора (после успешного сохранения)</summary>
        public int? AddedOrEditedAuthorId { get; private set; }

        public AuthorWindow(LibraryContext context, int? authorId = null)
        {
            InitializeComponent();
            _context = context;
            _authorId = authorId;

            if (_authorId.HasValue)
            {
                Title = "Редактирование автора";
                LoadAuthorData();
            }
            else
            {
                Title = "Добавление автора";
                BirthDatePicker.SelectedDate = DateTime.Now.AddYears(-30);
            }
        }

        private void LoadAuthorData()
        {
            var author = _context.Authors.Find(_authorId);
            if (author != null)
            {
                FirstNameBox.Text = author.FirstName;
                LastNameBox.Text = author.LastName;
                BirthDatePicker.SelectedDate = new DateTime(author.BirthDate.Year, author.BirthDate.Month, author.BirthDate.Day);
                CountryBox.Text = author.Country;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FirstNameBox.Text) ||
                string.IsNullOrWhiteSpace(LastNameBox.Text) ||
                BirthDatePicker.SelectedDate == null ||
                string.IsNullOrWhiteSpace(CountryBox.Text))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            try
            {
                var birthDate = DateTime.SpecifyKind(BirthDatePicker.SelectedDate!.Value.Date, DateTimeKind.Utc);

                Author author;
                if (_authorId.HasValue)
                {
                    author = _context.Authors.Find(_authorId)!;
                    if (author != null)
                    {
                        author.FirstName = FirstNameBox.Text;
                        author.LastName = LastNameBox.Text;
                        author.BirthDate = birthDate;
                        author.Country = CountryBox.Text;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    author = new Author
                    {
                        FirstName = FirstNameBox.Text,
                        LastName = LastNameBox.Text,
                        BirthDate = birthDate,
                        Country = CountryBox.Text
                    };
                    _context.Authors.Add(author);
                }

                _context.SaveChanges();
                AddedOrEditedAuthorId = author.Id;
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
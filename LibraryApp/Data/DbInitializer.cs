using LibraryApp.Models;

namespace LibraryApp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(LibraryContext context)
        {
            context.Database.EnsureCreated();

            // Добавляем жанры, если их нет
            if (!context.Genres.Any())
            {
                var genres = new Genre[]
                {
                    new Genre { Name = "Роман", Description = "Художественное произведение" },
                    new Genre { Name = "Фантастика", Description = "Научная фантастика и фэнтези" },
                    new Genre { Name = "Детектив", Description = "Детективные романы" },
                    new Genre { Name = "Поэзия", Description = "Стихотворные произведения" }
                };
                context.Genres.AddRange(genres);
                context.SaveChanges();
            }

            // Добавляем авторов, если их нет
            if (!context.Authors.Any())
            {
                var authors = new Author[]
                {
                    new Author { FirstName = "Лев", LastName = "Толстой", BirthDate = DateTime.SpecifyKind(new DateTime(1828, 9, 9), DateTimeKind.Utc), Country = "Россия" },
                    new Author { FirstName = "Федор", LastName = "Достоевский", BirthDate = DateTime.SpecifyKind(new DateTime(1821, 11, 11), DateTimeKind.Utc), Country = "Россия" },
                    new Author { FirstName = "Артур", LastName = "Конан Дойл", BirthDate = DateTime.SpecifyKind(new DateTime(1859, 5, 22), DateTimeKind.Utc), Country = "Великобритания" },
                    new Author { FirstName = "Агата", LastName = "Кристи", BirthDate = DateTime.SpecifyKind(new DateTime(1890, 9, 15), DateTimeKind.Utc), Country = "Великобритания" }
                };
                context.Authors.AddRange(authors);
                context.SaveChanges();
            }

            // Добавляем книги, если их нет
            if (!context.Books.Any())
            {
                var books = new Book[]
                {
                    new Book { 
                        Title = "Война и мир", 
                        PublishYear = 1869, 
                        ISBN = "978-5-17-123456-7", 
                        QuantityInStock = 5,
                        AuthorId = 1,
                        GenreId = 1
                    },
                    new Book { 
                        Title = "Преступление и наказание", 
                        PublishYear = 1866, 
                        ISBN = "978-5-04-123456-8", 
                        QuantityInStock = 3,
                        AuthorId = 2,
                        GenreId = 1
                    },
                    new Book { 
                        Title = "Приключения Шерлока Холмса", 
                        PublishYear = 1892, 
                        ISBN = "978-5-699-12345-6", 
                        QuantityInStock = 7,
                        AuthorId = 3,
                        GenreId = 3
                    },
                    new Book { 
                        Title = "Убийство в Восточном экспрессе", 
                        PublishYear = 1934, 
                        ISBN = "978-5-04-987654-3", 
                        QuantityInStock = 2,
                        AuthorId = 4,
                        GenreId = 3
                    }
                };
                context.Books.AddRange(books);
                context.SaveChanges();
            }
        }
    }
}
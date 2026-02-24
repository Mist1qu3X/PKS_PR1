using System;
using LibraryApp.Data;

namespace LibraryApp;

public static class TestDb
{
    public static void Test()
    {
        try
        {
            Console.WriteLine("Проверка подключения к БД...");
            using (var db = new LibraryContext())
            {
                var canConnect = db.Database.CanConnect();
                Console.WriteLine($"Могу подключиться: {canConnect}");
                
                if (canConnect)
                {
                    var count = db.Books.Count();
                    Console.WriteLine($"Количество книг: {count}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}
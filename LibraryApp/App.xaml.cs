using System.Windows;
using LibraryApp.Data;

namespace LibraryApp;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
        // Инициализация базы данных
        using (var db = new LibraryContext())
        {
            DbInitializer.Initialize(db);
        }
    }
}

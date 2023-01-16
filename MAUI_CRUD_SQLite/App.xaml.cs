using MAUI_CRUD_SQLite.Models;
namespace MAUI_CRUD_SQLite;

public partial class App : Application
{
    static SQLiteHelper db;
    public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
    public static SQLiteHelper SQLiteDb
    {
        get
        {
            if (db == null)
            {
                db = new SQLiteHelper(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MAUICRUD.db3"));
            }
            return db;
        }
    }
}

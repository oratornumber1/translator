using System;
using System.IO;
using CrossPlatformAppXamarin.Interfaces;
using Xamarin.Forms;
using CrossPlatformAppXamarin.iOS;

[assembly: Dependency(typeof(SQLite_iOS))]
namespace CrossPlatformAppXamarin.iOS
{
    public class SQLite_iOS : ISQLite
    {
        public SQLite_iOS() { }
        public string GetDatabasePath(string sqliteFilename)
        {
            // определяем путь к бд
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // папка библиотеки
            var path = Path.Combine(libraryPath, sqliteFilename);

            return path;
        }
    }
}
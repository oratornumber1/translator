using CrossPlatformAppXamarin.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using CrossPlatformAppXamarin.Models;

namespace CrossPlatformAppXamarin.Repositories
{
    public class TranslationRepository
    {
        SQLiteConnection database;
        public TranslationRepository(string filename)
        {
            string databasePath = DependencyService.Get<ISQLite>().GetDatabasePath(filename);
            database = new SQLiteConnection(databasePath);
            database.CreateTable<Models.Translation>();
        }
        public IEnumerable<Models.Translation> GetItems()
        {
            return (from i in database.Table<Models.Translation>() select i).ToList();

        }
        public Models.Translation GetItem(int id)
        {
            return database.Get<Models.Translation>(id);
        }
        public int DeleteItem(int id)
        {
            return database.Delete<Models.Translation>(id);
        }
        public int SaveItem(Models.Translation item)
        {
            if (item.Id != 0)
            {
                database.Update(item);
                return item.Id;
            }
            else
            {
                return database.Insert(item);
            }
        }
    }
}

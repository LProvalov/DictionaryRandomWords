using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using Xamarin.Forms;

namespace RandomWordsApp
{
    public class WordRepository
    {
        public enum WordPropertyName
        {
            Rus,
            Eng
        }
        static object locker = new object();
        SQLiteConnection database;
        public WordRepository(string filename)
        {
            Debug.WriteLine("WordRepository, constructor");
            string databasePath = DependencyService.Get<ISQLite>().GetDatabasePath(filename);
            Debug.WriteLine(string.Format("WordRepository, {0}", databasePath));
            database = new SQLiteConnection(databasePath);
            Debug.WriteLine(string.Format("WordRepository, db created"));
            database.CreateTable<Word>();
            Debug.WriteLine("WordRepository, table added");
        }

        public int GetLength()
        {
            Debug.WriteLine("WordRepository, getLength");
            int ret = database.Table<Word>().Count();
            Debug.WriteLine(string.Format("WordRepository, getLength return: {0}", ret));
            return ret;
        }

        public IList<Word> GetItems()
        {
            Debug.WriteLine("WordRepository, getItems");
            return database.Table<Word>().Select(a => a).ToList();
        }

        public IList<Word> GetPart(int partSize, int skip)
        {
            Debug.WriteLine(string.Format("WordRepository, getPart({0}, {1})", partSize, skip));
            return database.Table<Word>().Skip(skip).Take(partSize).ToList();
        }

        public Word GetItem(int itemId)
        {
            Debug.WriteLine(string.Format("WordRepository, getItem({0})", itemId));
            return database.Get<Word>(itemId);
        }

        public IList<Word> GetWordStartWith(string start, WordPropertyName propertyName)
        {
            Debug.WriteLine(string.Format("WordRepository, GetWordWithFirstA({0}, {1})", start, propertyName.ToString()));
            switch (propertyName)
            {
                case WordPropertyName.Rus:
                    var filtered = from s in database.Table<Word>()
                                   where s.Rus.ToUpper().StartsWith(start.ToUpper())
                                   select s;
                    return filtered.ToList();
                case WordPropertyName.Eng:
                    return database.Table<Word>().Select(a => a).Where(w => w.Eng.ToUpper().StartsWith(start.ToUpper())).ToList();
                default:
                    return null;
            }
        }

        public int DeleteItem(int itemId)
        {
            Debug.WriteLine("WordRepository, deleteItem");
            lock (locker)
            {
                return database.Delete<Word>(itemId);
            }
        }

        public int SaveItem(Word item)
        {
            Debug.WriteLine("WordRepository, saveItem");
            lock (locker)
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
}

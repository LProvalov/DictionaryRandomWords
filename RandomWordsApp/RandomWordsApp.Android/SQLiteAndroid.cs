using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RandomWordsApp;

using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(RandomWordsApp.Droid.SQLiteAndroid))]
namespace RandomWordsApp.Droid
{
    public class SQLiteAndroid : ISQLite
    {
        public SQLiteAndroid() { }
        public string GetDatabasePath(string filename)
        {
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string path = Path.Combine(documentsPath, filename);
            return path;
        }
    }
}
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using crud_xamarin_android.Core.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Environment;

namespace crud_xamarin_android.Core.Repositories
{
    internal class BaseRepository
    {
        public SQLiteConnection Connection { get; }
        private readonly TestData _data;

        public BaseRepository()
        {
            _data = new TestData();
            var pathDb = System.IO.Path.Combine(GetFolderPath(SpecialFolder.MyDocuments), "crud_xamarin_android.db");
            Connection = new SQLiteConnection(pathDb);
            
            Connection.DropTable<Article>();
            Connection.DropTable<Category>();
            Connection.CreateTable<Article>();
            Connection.CreateTable<Category>();
            Connection.InsertAll(_data.GetArticles(), typeof(Article));
            Connection.InsertAll(_data.GetCategories(), typeof(Category));
        }
    }
}
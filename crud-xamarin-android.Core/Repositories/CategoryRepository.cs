using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using crud_xamarin_android.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crud_xamarin_android.Core.Repositories
{
    internal class CategoryRepository : BaseRepository, IBaseRepository<Category>
    {
        public CategoryRepository() : base()
        {
        }

        private void checkTablesExist()
        {
            Connection.CreateTable<Category>();
        }






        public void Delete(int id)
        {
            checkTablesExist();
            Connection.Delete<Category>(id);
        }

        public IEnumerable<Category> GetAll()
        {
            checkTablesExist();
            var items = Connection.Table<Category>().ToList();
            return items;
        }

        public Category GetById(int id)
        {
            checkTablesExist();
            var item = Connection.Get<Category>(id);
            return item;
        }

        public void Insert(Category item)
        {
            checkTablesExist();
            Connection.Insert(item);
        }

        public int Update(Category item)
        {
            checkTablesExist();
            return Connection.Update(item, typeof(Category));
        }
    }
}
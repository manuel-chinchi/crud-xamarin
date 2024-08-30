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
    internal class ArticleRepository : BaseRepository, IBaseRepository<Article>
    {
        public ArticleRepository() : base()
        {
        }

        private void checkTablesExist()
        {
            Connection.CreateTable<Category>();
            Connection.CreateTable<Article>();
        }

        public void Delete(int id)
        {
            checkTablesExist();
            Connection.Delete<Article>(id);
        }

        public IEnumerable<Article> GetAll()
        {
            checkTablesExist();
            //var items = Connection.Table<Article>().ToList();
            //return items ?? null;

            var articles = Connection.Table<Article>();
            var categories = Connection.Table<Category>();
            var query =
                from article in articles
                join category in categories
                on article.CategoryId equals category.Id
                select new Article
                {
                    Id = article.Id,
                    Name = article.Name,
                    Details = article.Details,
                    Category = category
                };
            return query ?? null;
        }

        public Article GetById(int id)
        {
            checkTablesExist();
            var item = Connection.Find<Article>(id);
            return item ?? null;
        }

        public void Insert(Article item)
        {
            checkTablesExist();
            if (item.Category != null)
            {
                Connection.Insert(item.Category);
            }
            Connection.Insert(item);
        }

        public int Update(Article item)
        {
            checkTablesExist();
            return Connection.Update(item, typeof(Article));
        }
    }
}
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
        public ArticleRepository()
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
            var article = Connection.Find<Article>(id);
            var category = Connection.Find<Category>(article.CategoryId);
            category.Articles--;
            Connection.Update(category, typeof(Category));
            Connection.Delete<Article>(id);
        }

        public IEnumerable<Article> GetAll()
        {
            checkTablesExist();
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
                    Category = category,
                    CategoryId = category.Id
                };
            return query ?? null;
        }

        public Article GetById(int id)
        {
            checkTablesExist();
            var article = Connection.Find<Article>(id);
            return article ?? null;
        }

        public void Insert(Article article)
        {
            checkTablesExist();
            var category=Connection.Find<Category>(article.CategoryId);
            category.Articles++;
            Connection.Update(category, typeof(Category));
            Connection.Insert(article);
        }

        public int Update(Article article)
        {
            checkTablesExist();
            var oldArticle = Connection.Find<Article>(article.Id);
            if (oldArticle.CategoryId != article.CategoryId)
            {
                var oldCategory = Connection.Find<Category>(oldArticle.CategoryId);
                oldCategory.Articles--;
                Connection.Update(oldCategory, typeof(Category));
                var category = Connection.Find<Category>(article.CategoryId);
                category.Articles++;
                Connection.Update(category, typeof(Category));
            }
            return Connection.Update(article, typeof(Article));
        }
    }
}
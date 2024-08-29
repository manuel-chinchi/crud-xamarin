using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using crud_xamarin_android.Core.Models;
using crud_xamarin_android.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crud_xamarin_android.Core.Services
{
    public class ArticleService
    {
        ArticleRepository repository;
        

        public ArticleService()
        {
            repository = new ArticleRepository();
        }

        public IEnumerable<Article> GetArticles()
        {
            //return articles;
            return repository.GetAll();
        }

        public Article GetArticleById(int id)
        {
            //return articles.Where(a => a.Id == id).FirstOrDefault();
            return repository.GetById(id);
        }

        public void AddArticle(Article article)
        {
            //var id = -1;
            //if (articles.Count == 0)
            //{
            //    id = 1;
            //}
            //else
            //{
            //    id = articles.Max(a => a.Id) + 1;
            //}
            //article.Id = id;
            //articles.Add(article);
            repository.Insert(article);
        }

        public void DeleteArticle(int id)
        {
            //var article = articles.FirstOrDefault(a => a.Id == id);
            //articles.Remove(article);
            repository.Delete(id);
        }

        public void UpdateArticle(Article article)
        {
            //var a = articles.FirstOrDefault(a => a.Id == article.Id);
            //if (a != null)
            //{
            //    a.Name = article.Name;
            //    a.Details = article.Details;
            //}
            repository.Update(article);
        }
    }
}
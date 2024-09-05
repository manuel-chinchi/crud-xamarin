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
        private readonly ArticleRepository repository;

        public ArticleService()
        {
            repository = new ArticleRepository();
        }

        public IEnumerable<Article> GetArticles()
        {
            return repository.GetAll();
        }

        public Article GetArticleById(int id)
        {
            return repository.GetById(id);
        }

        public void AddArticle(Article article)
        {
            repository.Insert(article);
        }

        public void DeleteArticle(int id)
        {
            repository.Delete(id);
        }

        public void UpdateArticle(Article article)
        {
            repository.Update(article);
        }
    }
}
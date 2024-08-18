using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using crud_xamarin.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crud_xamarin.Core.Services
{
    public class ArticleService
    {
        private static List<Article> articles = new List<Article>()
        {
            new Article{Id=1, Name="a1"},
            new Article{Id=2, Name="a2"},
            new Article{Id=3, Name="a3"}
        };

        public ArticleService()
        {
        }

        public List<Article> GetArticles()
        {
            return articles;
        }

        public Article GetArticleById(int id)
        {
            return articles.Where(a => a.Id == id).FirstOrDefault();
        }
    }
}
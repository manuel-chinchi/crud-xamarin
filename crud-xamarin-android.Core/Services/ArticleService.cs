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

namespace crud_xamarin_android.Core.Services
{
    public class ArticleService
    {
        private static List<Article> articles = new List<Article>()
        {
            new Article{Id=1, Name="zapatilla", Details="talle 40, cuero"},
            new Article{Id=2, Name="zapatilla", Details="talle 42, cuero, mujer"},
            new Article{Id=3, Name="camisa", Details="talle L"},
            new Article{Id=4, Name="camisa", Details="talle M, lisa"},
            new Article{Id=5, Name="jean", Details="talle 41-43 chupin"},
            new Article{Id=6, Name="gorra", Details="varios colores lisas"},
            new Article{Id=7, Name="medias", Details="medianas"},
            new Article{Id=8, Name="zapato", Details="unisex, talle 40, sintético"},
            new Article{Id=9, Name="zapato", Details="talle 48"},
            new Article{Id=10, Name="gorra", Details="lisa blanca"},
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

        public void AddArticle(Article article)
        {
            var id = -1;
            if (articles.Count == 0)
            {
                id = 1;
            }
            else
            {
                id = articles.Max(a => a.Id) + 1;
            }
            article.Id = id;
            articles.Add(article);
        }

        public void DeleteArticle(int id)
        {
            var article = articles.FirstOrDefault(a => a.Id == id);
            articles.Remove(article);
        }

        public void UpdateArticle(Article article)
        {
            var a = articles.FirstOrDefault(a => a.Id == article.Id);
            if (a != null)
            {
                a.Name = article.Name;
                a.Details = article.Details;
            }
        }
    }
}
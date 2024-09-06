﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using crud_xamarin_android.Core.Helpers;
using crud_xamarin_android.Core.Models;
using crud_xamarin_android.Core.Repositories;
using crud_xamarin_android.Core.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crud_xamarin_android.Core.Services
{
    public class ArticleService
    {
        private readonly IArticleRepository articleRepository;
        private readonly ICategoryRepository categoryRepository;
        private Category untrackedCategory;

        public ArticleService()
        {
            articleRepository = new ArticleRepository();
            categoryRepository = new CategoryRepository();
            untrackedCategory = new Category
            {
                Id = CategoryHelper.ID_EMPTY_CATEGORY,
                Name = CategoryHelper.NAME_EMPTY_CATEGORY
            };
        }

        public IEnumerable<Article> GetArticles()
        {
            var articles = articleRepository.GetAll().ToList();

            for (int i = 0; i < articles.Count; i++)
            {
                if (articles[i].Category == null && articles[i].CategoryId != untrackedCategory.Id)
                {
                    articles[i].Category = categoryRepository.GetById(articles[i].CategoryId);
                }
                else if (articles[i].CategoryId == untrackedCategory.Id)
                {
                    articles[i].Category = untrackedCategory;
                }
            }

            return articles;
        }

        public Article GetArticleById(int id)
        {
            var article = articleRepository.GetById(id);
            var category = categoryRepository.GetById(article.CategoryId);

            if (category != null)
            {
                article.Category = category;
            }

            return article;
        }

        public void AddArticle(Article article)
        {
            var category = categoryRepository.GetById(article.CategoryId);

            if (category != null)
            {
                category.ArticleCount++;
                categoryRepository.Update(category);
            }

            articleRepository.Insert(article);
        }

        public void DeleteArticle(int id)
        {
            var article = articleRepository.GetById(id);
            var category = categoryRepository.GetById(article.CategoryId);

            if (category != null)
            {
                category.ArticleCount--;
                categoryRepository.Update(category);
            }

            articleRepository.Delete(id);
        }

        public void UpdateArticle(Article article)
        {
            var oldArticle = articleRepository.GetById(article.Id);

            if (oldArticle.CategoryId != article.CategoryId)
            {
                if (article.CategoryId != untrackedCategory.Id)
                {
                    var oldCategory = categoryRepository.GetById(oldArticle.CategoryId);
                    if (oldCategory != null)
                    {
                        oldCategory.ArticleCount--;
                        categoryRepository.Update(oldCategory);
                    }
                }

                var category = categoryRepository.GetById(article.CategoryId);
                category.ArticleCount++;
                categoryRepository.Update(category);
            }

            articleRepository.Update(article);
        }
    }
}
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using crud_xamarin_android.Core.Models;
using crud_xamarin_android.Core.Repositories;
using crud_xamarin_android.Core.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crud_xamarin_android.Core.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IArticleRepository articleRepository;
        private readonly Category _emptyCategory;
        public Category EmptyCategory { get { return _emptyCategory; } }

        public CategoryService()
        {
            categoryRepository = new CategoryRepository();
            articleRepository = new ArticleRepository();
            _emptyCategory = new Category { Id = 0, Name = "UNCATEGORIZED" };
        }

        public IEnumerable<Category> GetCategories()
        {
            var categories = categoryRepository.GetAll().ToList();
            var articles = articleRepository.GetAll();

            for (int i = 0; i < categories.Count; i++)
            {
                if (categories[i].ArticleCount > 0)
                {
                    categories[i].Articles = articles.Where(a => a.CategoryId == categories[i].Id).ToList();
                }
            }

            return categories;
        }

        public Category GetCategoryById(int id)
        {
            var category = categoryRepository.GetById(id);
            var articles = articleRepository.GetAll();
            category.Articles = articles.Where(a => a.CategoryId == category.Id).ToList();
            return category;
        }

        public void AddCategory(Category category)
        {
            categoryRepository.Insert(category);
        }

        public void UpdateCategory(Category category)
        {
            categoryRepository.Update(category);
        }

        public void DeleteCategory(int id)
        {
            var category = categoryRepository.GetById(id);
            var articles = articleRepository.GetAll().Where(a=>a.CategoryId == category.Id).ToList();

            if (articles != null && articles.Count > 0)
            {
                foreach (var article in articles)
                {
                    article.CategoryId = EmptyCategory.Id;
                    articleRepository.Update(article);
                }
            }

            categoryRepository.Delete(id);
        }

        public static bool IsEmptyCategory(int id)
        {
            return id == 0 ? true : false;
        }
    }
}
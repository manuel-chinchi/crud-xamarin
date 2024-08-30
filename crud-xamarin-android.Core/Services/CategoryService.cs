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
    public class CategoryService
    {
        private readonly CategoryRepository repository;

        public CategoryService()
        {
            repository = new CategoryRepository();
        }

        public IEnumerable<Category> GetCategories()
        {
            return repository.GetAll();
        }

        public Category GetCategoryById(int id)
        {
            return repository.GetById(id);
        }

        public void AddCategory(Category category)
        {
            repository.Insert(category);
        }

        public void UpdateCategory(Category category)
        {
            repository.Update(category);
        }

        public void DeleteCategory(int id)
        {
            repository.Delete(id);
        }
    }
}
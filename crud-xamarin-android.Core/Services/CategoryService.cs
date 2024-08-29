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
            //return categories;
            return repository.GetAll();
        }

        public Category GetCategoryById(int id)
        {
            //return categories.FirstOrDefault(c => c.Id == id);
            return repository.GetById(id);
        }

        public void AddCategory(Category category)
        {
            //int id = -1;
            //if (categories.Count == 0)
            //{
            //    id = 1;
            //}
            //else
            //{
            //    id = categories.Max(c=>c.Id)+1;
            //}
            //category.Id = id;
            //categories.Add(category);
            repository.Insert(category);
        }

        public void UpdateCategory(Category category)
        {
            //var categoryToEdit = categories.FirstOrDefault(c => c.Id == category.Id);
            //if (categoryToEdit!=null)
            //{
            //    categoryToEdit.Name = category.Name;
            //}
            repository.Update(category);
        }

        public void DeleteCategory(int id)
        {
            //var category = categories.FirstOrDefault(c => c.Id == id);
            //categories.Remove(category);
            repository.Delete(id);
        }
    }
}
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
    public class CategoryService
    {
        static List<Category> categories = new List<Category>
        {
            new Category { Id = 1, Name = "Otro" },
            new Category { Id = 2, Name = "Pantalones" },
            new Category { Id = 3, Name = "Remeras" },
            new Category { Id = 4, Name = "Camisas" },
            new Category { Id = 5, Name = "Busos" },
            new Category { Id = 6, Name = "Gorras" },
        };

        public CategoryService()
        {
        }

        public List<Category> GetCategories()
        {
            return categories;
        }

        public Category GetCategoryById(int id)
        {
            return categories.FirstOrDefault(c => c.Id == id);
        }

        public void AddCategory(Category category)
        {
            int id = -1;
            if (categories.Count == 0)
            {
                id = 1;
            }
            else
            {
                id = categories.Max(c=>c.Id)+1;
            }
            category.Id = id;
            categories.Add(category);
        }

        public void UpdateCategory(Category category)
        {
            var categoryToEdit = categories.FirstOrDefault(c => c.Id == category.Id);
            if (categoryToEdit!=null)
            {
                categoryToEdit.Name = category.Name;
            }
        }

        public void DeleteCategory(int id)
        {
            var category = categories.FirstOrDefault(c => c.Id == id);
            categories.Remove(category);
        }
    }
}
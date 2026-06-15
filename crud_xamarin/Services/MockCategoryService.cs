using crud_xamarin.Models;
using crud_xamarin.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace crud_xamarin.Services
{
    public class MockCategoryService : ICategoryService<CategoryModel>
    {
        private static List<CategoryModel> s_categories;

        public MockCategoryService()
        {
            s_categories = new List<CategoryModel>()
            {
                new CategoryModel() { Id = 200, Name = "Remeras H/M", Code = "REMS-U" },
                new CategoryModel() { Id = 201, Name = "Jeans Clasico", Code = "JEAN-C"},
                new CategoryModel() { Id = 202, Name = "Buzos Clasico", Code = "BUZS-C"}
            };
        }

        public async Task CreateCategoryAsync(CategoryModel item)
        {
            item.Id = GlobalHelper.NewId();
            s_categories.Add(item);
            await Task.CompletedTask;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var indexItem = s_categories.FindIndex(c => c.Id == id);
            if (indexItem > 0)
            {
                s_categories.RemoveAt(indexItem);
            }
            await Task.CompletedTask;
        }

        public async Task<List<CategoryModel>> GetCategoriesAsync()
        {
            return await Task.FromResult(s_categories);
        }

        public async Task UpdateCategoryAsync(CategoryModel item)
        {
            var editItem = s_categories.Find(c => c.Id == item.Id);
            if (editItem != null)
            {
                editItem.Name = item.Name;
                editItem.Code = item.Code;
            }
            await Task.CompletedTask;
        }

    }
}

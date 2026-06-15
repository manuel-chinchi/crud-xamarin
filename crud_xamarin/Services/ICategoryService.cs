using crud_xamarin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace crud_xamarin.Services
{
    public interface ICategoryService<T> where T:class
    {
        Task<List<T>> GetCategoriesAsync();
        Task CreateCategoryAsync(T item);
        Task UpdateCategoryAsync(T item);
        Task DeleteCategoryAsync(int id);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace crud_xamarin.Services
{
    interface IProductService<T>
    {
        Task<bool> AddProductAsync(T item);
        Task<bool> UpdateProductAsync(T item);
        Task<bool> DeleteProductAsync(string id);
        Task<IEnumerable<T>> GetProductsAsync();
        Task<T> GetProductById(string id);
    }
}

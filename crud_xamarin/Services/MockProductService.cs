using crud_xamarin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crud_xamarin.Services
{
    class MockProductService : IProductService<ProductModel>
    {
        private readonly List<ProductModel> s_products;

        public MockProductService()
        {
            s_products = new List<ProductModel>()
            {
                new ProductModel() { Id = 100, Name = "zapatilla", Description = "Nike" },
                new ProductModel() { Id = 101, Name = "remera lisa", Description = "Adidas" },
                new ProductModel() { Id = 102, Name = "remera", Description = "China" },
                new ProductModel() { Id = 103, Name = "camisa", Description = "China" },
            };
        }

        public Task<bool> AddProductAsync(ProductModel item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteProductAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductModel> GetProductById(string id)
        {
            var products = await Task.FromResult(s_products);
            var id2 = Convert.ToInt32(id);
            var product = products.Where(p => p.Id == id2).FirstOrDefault();

            return product;
        }

        public async Task<IEnumerable<ProductModel>> GetProductsAsync()
        {
            return await Task.FromResult(s_products);
        }

        public Task<bool> UpdateProductAsync(ProductModel item)
        {
            throw new NotImplementedException();
        }
    }
}

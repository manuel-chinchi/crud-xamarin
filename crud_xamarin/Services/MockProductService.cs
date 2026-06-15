using crud_xamarin.Models;
using crud_xamarin.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crud_xamarin.Services
{
    public class MockProductService : IProductService<ProductModel>
    {
        private static List<ProductModel> s_products;

        public MockProductService()
        {
            s_products = new List<ProductModel>()
            {
                new ProductModel() { Id = 100, Name = "zapatilla", Description = "Nike" },
                new ProductModel() { Id = 101, Name = "remera lisa", Description = "Adidas" },
                new ProductModel() { Id = 102, Name = "remera", Description = "China" },
                new ProductModel() { Id = 103, Name = "camisa", Description = "China" },
                new ProductModel() 
                {
                    Id = 104,
                    Name = "gorra",
                    Description = "Lisa",
                    Category = new CategoryModel()
                    {
                        Id = 105,
                        Name = "indumentaria",
                        Code = "INDU-00"
                    }
                }
            };
        }

        public Task<bool> AddProductAsync(ProductModel item)
        {
            item.Id = GlobalHelper.NewId();
            s_products.Add(item);
            return Task.FromResult(true);
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

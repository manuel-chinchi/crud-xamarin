using crud_xamarin.Models;
using crud_xamarin.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace crud_xamarin.ViewModels
{
    class ProductsViewModel : BaseViewModel
    {
        private ObservableCollection<ProductModel> _products;
        public ObservableCollection<ProductModel> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        private readonly IProductService<ProductModel> _productService;

        public ProductsViewModel()
        {
            _productService = DependencyService.Get<IProductService<ProductModel>>();

            LoadProducts();
        }

        public async Task LoadProducts()
        {
            var items = await _productService.GetProductsAsync();

            Products = new ObservableCollection<ProductModel>(items);
        }
    }
}

using crud_xamarin.Models;
using crud_xamarin.Services;
using crud_xamarin.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace crud_xamarin.ViewModels
{
    class ProductCreateViewModel: BaseViewModel
    {
        private ProductModel _product = new ProductModel();
        public ProductModel Product
        {
            get => _product;
            set => SetProperty(ref _product, value);
        }

        private readonly IProductService<ProductModel> _productService;

        public ICommand CreateProductCommand { get; }

        public ProductCreateViewModel()
        {
            _productService = DependencyService.Get<IProductService<ProductModel>>();

            CreateProductCommand = new Command<ProductModel>(CreateProduct);
        }

        private async void CreateProduct(ProductModel product)
        {
            if (product != null)
            {
                _ = await _productService.AddProductAsync(product);

                await Shell.Current.GoToAsync($"{nameof(ProductsView)}");
            }
        }
    }
}

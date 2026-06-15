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
    public class ProductCreateViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

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
            CreateProductCommand = new Command<ProductModel>(CreateProduct);
        }

        public ProductCreateViewModel(IProductService<ProductModel> productService, INavigationService navigationService)
        {
            _productService = productService;
            _navigationService = navigationService;

            CreateProductCommand = new Command<ProductModel>(CreateProduct);
        }

        private async void CreateProduct(ProductModel product)
        {
            if (product != null)
            {
                _ = await _productService.AddProductAsync(product);

                await _navigationService.PushAsync<ProductsView>(clearStack: true);
            }
        }
    }
}

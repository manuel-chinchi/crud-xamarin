using crud_xamarin.Models;
using crud_xamarin.Services;
using crud_xamarin.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace crud_xamarin.ViewModels
{
    public class ProductsViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        private ObservableCollection<ProductModel> _products;
        public ObservableCollection<ProductModel> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        private readonly IProductService<ProductModel> _productService;

        public ICommand GoToProductDetailCommand { get; }
        public ICommand CreateProductCommand { get; }

        public ProductsViewModel()
        {
            GoToProductDetailCommand = new Command<ProductModel>(ViewProductDetails);
            CreateProductCommand = new Command(CreateProduct);
        }

        public ProductsViewModel(IProductService<ProductModel> productService, INavigationService navigationService)
        {
            _productService = productService;
            _navigationService = navigationService;

            GoToProductDetailCommand = new Command<ProductModel>(ViewProductDetails);
            CreateProductCommand = new Command(CreateProduct);
        }

        private async void ViewProductDetails(ProductModel product)
        {
            if (product == null)
                return;

            await _navigationService.PushAsync<ProductDetailView, ProductDetailViewModel>(viewModel =>
            {
                viewModel.Id = product.Id;
            });
        }

        public async Task LoadProducts()
        {
            var items = await _productService.GetProductsAsync();

            Products = new ObservableCollection<ProductModel>(items);
        }

        private async void CreateProduct(object obj)
        {
            await _navigationService.PushAsync<ProductCreateView>();
        }
    }
}

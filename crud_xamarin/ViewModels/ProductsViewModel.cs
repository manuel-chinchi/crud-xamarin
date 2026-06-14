using crud_xamarin.Models;
using crud_xamarin.Services;
using crud_xamarin.Views;
using Microsoft.Extensions.DependencyInjection;
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
            _productService = DependencyService.Get<IProductService<ProductModel>>();

            GoToProductDetailCommand = new Command<ProductModel>(ViewProductDetails);
            CreateProductCommand = new Command(CreateProduct);
        }

        public ProductsViewModel(IProductService<ProductModel> productService)
        {
            _productService = productService;

            GoToProductDetailCommand = new Command<ProductModel>(ViewProductDetails);
            CreateProductCommand = new Command(CreateProduct);
        }

        private async void ViewProductDetails(ProductModel product)
        {
            if (product == null)
                return;

            var view = App.Services.GetRequiredService<ProductDetailView>();
            ProductDetailViewModel viewModel = (ProductDetailViewModel)view.BindingContext;
            viewModel.Id = product.Id;
            await Shell.Current.Navigation.PushAsync(view);
        }

        public async Task LoadProducts()
        {
            var items = await _productService.GetProductsAsync();

            Products = new ObservableCollection<ProductModel>(items);
        }

        private async void CreateProduct(object obj)
        {
            var view = App.Services.GetRequiredService<ProductCreateView>();
            await Shell.Current.Navigation.PushAsync(view);
        }
    }
}

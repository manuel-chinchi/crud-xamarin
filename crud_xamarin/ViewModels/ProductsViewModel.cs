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
    class ProductsViewModel : BaseViewModel
    {
        private ObservableCollection<ProductModel> _products;
        public ObservableCollection<ProductModel> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        private readonly IProductService<ProductModel> _productService;

        public ICommand ViewProductDetailsCommand { get; }

        public ProductsViewModel()
        {
            _productService = DependencyService.Get<IProductService<ProductModel>>();

            ViewProductDetailsCommand = new Command<ProductModel>(ViewProductDetails);
        }

        private async void ViewProductDetails(ProductModel product)
        {
            if (product == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(ProductDetailView)}?Id={product.Id}");
        }

        public async Task LoadProducts()
        {
            var items = await _productService.GetProductsAsync();

            Products = new ObservableCollection<ProductModel>(items);
        }
    }
}

using crud_xamarin.Models;
using crud_xamarin.Services;
using crud_xamarin.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace crud_xamarin.ViewModels
{
    public class ProductDetailViewModel : BaseViewModel
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private ProductModel _product;
        public ProductModel Product
        {
            get => _product;
            set => SetProperty(ref _product, value);
        }

        private readonly IProductService<ProductModel> _productService;

        public ProductDetailViewModel()
        {
            _productService = DependencyService.Get<IProductService<ProductModel>>();
        }

        public ProductDetailViewModel(IProductService<ProductModel> productService)
        {
            _productService = productService;
        }

        public async Task LoadProduct()
        {
            var product = await _productService.GetProductById(Id.ToString());
            Product = product;
        }
    }
}

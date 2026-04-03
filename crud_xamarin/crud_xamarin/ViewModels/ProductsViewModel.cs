using crud_xamarin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace crud_xamarin.ViewModels
{
    class ProductsViewModel : BaseViewModel
    {
        private List<ProductModel> _products = new List<ProductModel>();
        public List<ProductModel> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        public ProductsViewModel()
        {

        }
    }
}

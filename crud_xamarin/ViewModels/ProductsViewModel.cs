using crud_xamarin.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

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

        public ProductsViewModel()
        {
            LoadProducts();
        }

        private void LoadProducts()
        {
            var items = new List<ProductModel>
            {
                new ProductModel
                {
                    Id = 100,
                    Name ="zapatilla",
                    Description = "H Talle 32"
                },
                new ProductModel
                {
                    Id=101,
                    Name="remera lisa",
                    Description = "H Talle M"
                },
                new ProductModel
                {
                    Id=102,
                    Name="pantalon deportivo",
                    Description = "Unisex"
                },
            };

            Products = new ObservableCollection<ProductModel>(items);
        }
    }
}

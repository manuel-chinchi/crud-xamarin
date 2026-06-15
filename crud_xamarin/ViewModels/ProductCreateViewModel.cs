using crud_xamarin.Models;
using crud_xamarin.Services;
using crud_xamarin.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace crud_xamarin.ViewModels
{
    public class ProductCreateViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IProductService<ProductModel> _productService;
        private readonly ICategoryService<CategoryModel> _categoryService;

        private ProductModel _product = new ProductModel();
        public ProductModel Product
        {
            get => _product;
            set => SetProperty(ref _product, value);
        }

        private CategoryModel _category = new CategoryModel();
        public CategoryModel Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        private ObservableCollection<CategoryModel> _categories;
        public ObservableCollection<CategoryModel> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        public ICommand CreateProductCommand { get; }

        public ProductCreateViewModel()
        {
            CreateProductCommand = new Command<ProductModel>(CreateProduct);
        }

        public ProductCreateViewModel(IProductService<ProductModel> productService, ICategoryService<CategoryModel> categoryService, INavigationService navigationService)
        {
            _productService = productService;
            _navigationService = navigationService;
            _categoryService = categoryService;

            CreateProductCommand = new Command<ProductModel>(CreateProduct);
        }

        private async void CreateProduct(ProductModel product)
        {
            if (product != null)
            {
                if (Category != null)
                {
                    // TODO agregar CategoryModel a ProductModel
                    product.Category = Category;
                }
                _ = await _productService.AddProductAsync(product);

                await _navigationService.PushAsync<ProductsView>();
            }
        }

        public async void OnLoadView()
        {
            var items = await _categoryService.GetCategoriesAsync();
            Categories = new ObservableCollection<CategoryModel>(items);
        }
    }
}

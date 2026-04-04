using crud_xamarin.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace crud_xamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsView : ContentPage
    {
        private readonly ProductsViewModel _viewModel;

        public ProductsView()
        {
            InitializeComponent();

            _viewModel = (ProductsViewModel)BindingContext;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await _viewModel.LoadProducts();
        }
    }
}

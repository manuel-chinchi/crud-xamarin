using crud_xamarin.Services;
using crud_xamarin.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace crud_xamarin
{
    public partial class MainPage : ContentPage
    {
        private readonly INavigationService _navigationService;

        public MainPage()
        {
            InitializeComponent();
        }

        public MainPage(INavigationService navigationService)
        {
            InitializeComponent();

            _navigationService = navigationService;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await _navigationService.PushAsync<ProductsView>();
        }
    }
}

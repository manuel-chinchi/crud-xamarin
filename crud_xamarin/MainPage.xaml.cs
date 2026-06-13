using crud_xamarin.Views;
using Microsoft.Extensions.DependencyInjection;
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
        public MainPage()
        {
            InitializeComponent();
        }

        private async void btnGoToProducts_Clicked(object sender, EventArgs e)
        {
            ProductsView view = App.Services.GetRequiredService<ProductsView>();

            await Shell.Current.Navigation.PushAsync(view);
        }
    }
}

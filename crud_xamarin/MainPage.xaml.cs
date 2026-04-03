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
        public MainPage()
        {
            InitializeComponent();
        }

        private async void btnGoToProducts_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(ProductsView)}");
        }
    }
}

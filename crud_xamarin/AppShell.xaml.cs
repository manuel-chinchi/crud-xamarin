using crud_xamarin.Services;
using crud_xamarin.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace crud_xamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            RegisterServices();
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(ProductsView), typeof(ProductsView));
            Routing.RegisterRoute(nameof(ProductDetailView), typeof(ProductDetailView));
        }

        private void RegisterServices()
        {
            DependencyService.Register<MockProductService>();
        }
    }
}
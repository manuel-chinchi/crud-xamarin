using crud_xamarin.Services;
using crud_xamarin.Views;
using Microsoft.Extensions.DependencyInjection;
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
            Routing.RegisterRoute(nameof(ProductCreateView), typeof(ProductCreateView));
        }

        private void RegisterServices()
        {
            DependencyService.Register<MockProductService>();
        }

        public void Initialize()
        {
            // this method configure DI in the MainPage constructor
            var mainPage = App.Services.GetRequiredService<MainPage>();
            var navigationPage = new NavigationPage(mainPage);

            NavigationPage.SetHasBackButton(navigationPage, false);

            this.Items.Clear();
            var shellContent = new ShellContent { Content = navigationPage };
            var shellSection = new ShellSection { Items = { shellContent } };
            var shellItem = new ShellItem { Items = { shellSection } };
            this.Items.Add(shellItem);
        }
    }
}
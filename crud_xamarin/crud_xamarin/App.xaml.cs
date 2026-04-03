using crud_xamarin.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace crud_xamarin
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            RegisterRoutes();

            MainPage = new AppShell();
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(ProductsView), typeof(ProductsView));
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

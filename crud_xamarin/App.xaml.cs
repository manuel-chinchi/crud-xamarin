using crud_xamarin.Models;
using crud_xamarin.Services;
using crud_xamarin.ViewModels;
using crud_xamarin.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace crud_xamarin
{
    public partial class App : Application
    {
        private static IServiceProvider _serviceProvider;
        public static IServiceProvider Services => _serviceProvider;

        public App()
        {
            InitializeComponent();

            #region Configure DI

            var services = new ServiceCollection();
            // services
            services.AddTransient<IProductService<ProductModel>, MockProductService>();

            // viewmodels
            services.AddTransient<ProductsViewModel>();

            // views
            services.AddTransient<ProductsView>();

            _serviceProvider = services.BuildServiceProvider();

            DependencyResolver.ResolveUsing(type =>
            {
                var service = _serviceProvider.GetService(type);
                return service;
            });

            #endregion

            MainPage = new AppShell();
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

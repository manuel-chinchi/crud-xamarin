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
        public static IServiceProvider Services { get; private set; }

        public App()
        {
            InitializeComponent();

            #region Configure DI

            var services = new ServiceCollection();

            // services
            services.AddTransient<IProductService<ProductModel>, MockProductService>();

            // viewmodels
            services.AddTransient<ProductsViewModel>();
            services.AddTransient<ProductCreateViewModel>();
            services.AddTransient<ProductDetailViewModel>();

            // views
            services.AddTransient<ProductsView>();
            services.AddTransient<ProductCreateView>();
            services.AddTransient<ProductDetailView>();

            Services = services.BuildServiceProvider();

            DependencyResolver.ResolveUsing(type =>
            {
                object service = Services.GetService(type);
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

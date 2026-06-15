using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace crud_xamarin.Services
{
    public sealed class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private Page GetCurrentPage()
        {
            Page mainPage = Application.Current.MainPage;

            if (mainPage is NavigationPage navigationPage)
            {
                return navigationPage.CurrentPage;
            }
            else if (mainPage is Shell shell)
            {
                return shell.CurrentPage;
            }

            return mainPage;
        }

        private async Task DefaultNavigateTo<TView>(TView view) where TView : ContentPage
        {
            var currentPage = GetCurrentPage();

            if (currentPage?.Navigation != null)
            {
                await currentPage.Navigation.PopToRootAsync(false);
                await currentPage.Navigation.PushAsync(view);
            }
            else if (currentPage?.Navigation != null)
            {
                await currentPage.Navigation.PushAsync(view);
            }
        }

        public async Task PushAsync<TView>() where TView : ContentPage
        {
            TView view = _serviceProvider.GetRequiredService<TView>();

            await DefaultNavigateTo(view);
        }

        public async Task PushAsync<TView, TViewModel>(Action<TViewModel> configViewModel) where TView : ContentPage
        {
            TView view = _serviceProvider.GetRequiredService<TView>();

            var viewModel = (TViewModel)view.BindingContext;
            configViewModel(viewModel);

            await DefaultNavigateTo(view);
        }
    }
}
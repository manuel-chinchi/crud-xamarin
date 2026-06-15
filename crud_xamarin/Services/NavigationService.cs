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
        public NavigationService()
        {
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

        private async Task DefaultNavigateTo<T>(T view) where T : Page
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
            TView view = App.Services.GetRequiredService<TView>();

            await DefaultNavigateTo(view);
        }

        public async Task PushAsync<TView, TViewModel>(Action<TViewModel> configViewModel) where TView : ContentPage
        {
            TView view = App.Services.GetRequiredService<TView>();

            var viewModel = (TViewModel)view.BindingContext;
            configViewModel(viewModel);

            await DefaultNavigateTo(view);
        }
    }
}
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

        public async Task PushAsync<TView>(bool clearStack) where TView : ContentPage
        {
            TView view = App.Services.GetRequiredService<TView>();

            var currentPage = GetCurrentPage();

            if (clearStack && currentPage?.Navigation != null)
            {
                // Limpiar la pila y navegar
                await currentPage.Navigation.PopToRootAsync(false);
                await currentPage.Navigation.PushAsync(view);
            }
            else if (currentPage?.Navigation != null)
            {
                await currentPage.Navigation.PushAsync(view);
            }
        }

        public async Task PushAsync<TView, TViewModel>(Action<TViewModel> configViewModel) where TView : ContentPage
        {
            TView view = App.Services.GetRequiredService<TView>();

            var viewModel = (TViewModel)view.BindingContext;
            configViewModel(viewModel);

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
    }
}
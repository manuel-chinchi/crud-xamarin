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

        public async Task PushAsync<TView>() where TView : ContentPage
        {
            TView view = App.Services.GetRequiredService<TView>();
            await Shell.Current.Navigation.PushAsync(view);
        }

        public async Task PushAsync<TView, TViewModel>(Action<TViewModel> configViewModel) where TView : ContentPage
        {
            TView view = App.Services.GetRequiredService<TView>();

            var viewModel = (TViewModel)view.BindingContext;
            configViewModel(viewModel);

            await Shell.Current.Navigation.PushAsync(view);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace crud_xamarin.Services
{
    public interface INavigationService
    {
        Task PushAsync<TView>(bool clearStack = false) where TView : ContentPage;
        Task PushAsync<TView, TViewModel>(Action<TViewModel> p) where TView : ContentPage;
    }
}

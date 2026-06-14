using crud_xamarin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace crud_xamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductCreateView : ContentPage
    {
        private readonly ProductCreateViewModel _viewModel;

        public ProductCreateView()
        {
            InitializeComponent();

            _viewModel = (ProductCreateViewModel)BindingContext;
        }

        public ProductCreateView(ProductCreateViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            BindingContext = viewModel;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace crud_xamarin.Components
{
    /// <summary>
    /// Control tipo <c>Picker</c> personalizado que permite uso de <c>Placeholder</c> (solo para Android) 
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PickerEx : ContentView
    {
        #region Private Properties
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(PickerEx), 
                default(IEnumerable));

        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(
                nameof(SelectedItem), 
                typeof(object), 
                typeof(PickerEx), 
                null,
                BindingMode.TwoWay, 
                propertyChanged: OnSelectedItemChanged);

        public static readonly BindableProperty PlaceholderTextProperty =
            BindableProperty.Create(
                nameof(PlaceholderText), 
                typeof(string), 
                typeof(PickerEx), 
                "Selecciona una categoría");

        public static readonly BindableProperty ItemDisplayProperty =
            BindableProperty.Create(
                nameof(ItemDisplay), 
                typeof(string), 
                typeof(PickerEx), 
                "Name");

        public static readonly BindableProperty ItemDisplayTemplateProperty =
            BindableProperty.Create(
                nameof(ItemDisplayTemplate), 
                typeof(DataTemplate), 
                typeof(PickerEx));

        public static readonly BindableProperty DisplayMemberPathProperty =
            BindableProperty.Create(
                nameof(DisplayMemberPath),
                typeof(string),
                typeof(PickerEx),
                "RandomValue",
                propertyChanged: OnDisplayMemberPathChanged);

        #endregion

        private static void OnDisplayMemberPathChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (PickerEx)bindable;
            var path = newValue as string;
            if (string.IsNullOrEmpty(path)) return;

            if (control.picSelectCategory != null)
                control.picSelectCategory.ItemDisplayBinding = new Binding(path);

            if (control.picSelectCategoryNative != null)
                control.picSelectCategoryNative.ItemDisplayBinding = new Binding(path);
        }

        #region Public Properties

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }

        public string ItemDisplay
        {
            get => (string)GetValue(ItemDisplayProperty);
            set => SetValue(ItemDisplayProperty, value);
        }

        public string DisplayMemberPath
        {
            get => (string)GetValue(DisplayMemberPathProperty);
            set => SetValue(DisplayMemberPathProperty, value);
        }

        public DataTemplate ItemDisplayTemplate
        {
            get => (DataTemplate)GetValue(ItemDisplayTemplateProperty);
            set => SetValue(ItemDisplayTemplateProperty, value);
        }

        #endregion

        public PickerEx()
        {
            InitializeComponent();
            this.Content.BindingContext = this;
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (PickerEx)bindable;
            control.UpdateLabelVisibility();
        }

        private void UpdateLabelVisibility()
        {
            if (lblSelectCategory != null && picSelectCategory != null)
            {
                lblSelectCategory.IsVisible = SelectedItem == null;
                picSelectCategory.IsVisible = SelectedItem != null;
            }
        }

        public event EventHandler<EventArgs> SelectedItemChanged;
        private void ctrlTouchSelectCategory_Tapped(object sender, EventArgs e)
        {
            // En Android: mostrar el picker oculto y enfocarlo
            if (Device.RuntimePlatform == Device.Android)
            {
                picSelectCategory.IsVisible = true;
                picSelectCategory.Focus();
                lblSelectCategory.IsVisible = false;
            }
        }

        private void picSelectCategory_Unfocused(object sender, FocusEventArgs e)
        {
            if (Device.RuntimePlatform == Device.Android && SelectedItem == null)
            {
                picSelectCategory.IsVisible = false;
                lblSelectCategory.IsVisible = true;
            }
        }
    }
}
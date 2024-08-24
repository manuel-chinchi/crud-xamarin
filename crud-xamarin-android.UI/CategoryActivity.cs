using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using crud_xamarin_android.Core.Services;
using crud_xamarin_android.UI.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crud_xamarin_android.UI
{
    [Activity(Label = "CategoryActivity")]
    public class CategoryActivity : Activity
    {
        RecyclerView recyclerView;
        CategoryAdapter adapter;
        CategoryService categoryService;
        public CategoryActivity()
        {
            categoryService = new CategoryService();
            adapter = new CategoryAdapter(categoryService.GetCategories());
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_category);

            recyclerView = FindViewById<RecyclerView>(Resource.Id.lstCategories);
            recyclerView.SetLayoutManager(new LinearLayoutManager(this));
            recyclerView.SetAdapter(adapter);

            var btnAddCategory = FindViewById<Button>(Resource.Id.btnAddCategory);
            btnAddCategory.Click += BtnAddCategory_Click;
            var btnDeleteCategory = FindViewById<Button>(Resource.Id.btnDeleteCategory);
            btnDeleteCategory.Click += BtnDeleteCategory_Click;
        }

        private void BtnAddCategory_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(CreateCategoryActivity));
            StartActivity(intent);
        }

        private void BtnDeleteCategory_Click(object sender, EventArgs e)
        {
        }
    }
}
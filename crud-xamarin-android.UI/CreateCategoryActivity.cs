using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using crud_xamarin_android.Core.Models;
using crud_xamarin_android.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crud_xamarin_android.UI
{
    [Activity(Label = "CreateCategoryActivity")]
    public class CreateCategoryActivity : Activity
    {
        CategoryService categoryService;
        public CreateCategoryActivity()
        {
            categoryService = new CategoryService();
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_create_category);

            var btnCancel = FindViewById<Button>(Resource.Id.btnCancel_Category);
            btnCancel.Click += BtnCancel_Click;
            var btnAccept = FindViewById<Button>(Resource.Id.btnAccept_Category);
            btnAccept.Click += BtnAccept_Click;
        }

        private void BtnAccept_Click(object sender, EventArgs e)
        {
            var toast = Toast.MakeText(this, "Category successfully added!", ToastLength.Short);
            toast.SetGravity(GravityFlags.Top | GravityFlags.CenterHorizontal, 0, 0);
            toast.Show();

            var inpNameCategory = FindViewById<EditText>(Resource.Id.inpNameCategory);
            categoryService.AddCategory(new Category { Name = inpNameCategory.Text });

            Finish();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }
}
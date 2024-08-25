using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using crud_xamarin_android.Core.Models;
using crud_xamarin_android.Core.Services;
using crud_xamarin_android.UI.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crud_xamarin_android.UI
{
    [Activity(Label = "CreateArticleActivity")]
    public class CreateArticleActivity : Activity
    {
        private ArticleService articleService = new ArticleService();
        CategoryService categoryService = new CategoryService();
        List<Category> categories;
        Category categorySelected;

        public CreateArticleActivity()
        {
            this.categories = categoryService.GetCategories();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_create_article);

            var btnCancel = FindViewById<Button>(Resource.Id.btnCancelar);
            var btnAccept = FindViewById<Button>(Resource.Id.btnAceptar);

            btnAccept.Click += BtnAccept_Click;
            btnCancel.Click += BtnCancel_Click;

            //var spnCategories = FindViewById<Spinner>(Resource.Id.spnCate);
            //var aa = Resource.Id.spnCategories;
            var spnCategories = FindViewById<Spinner>(Resource.Id.spnCategories);
            var categories = categoryService.GetCategories().OrderBy(c=>c.Name);
            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, categories.Select(c => c.Name).ToList());
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spnCategories.Adapter = adapter;

            spnCategories.ItemSelected += SpnCategories_ItemSelected;
        }

        private void SpnCategories_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            categorySelected = categories[e.Position];
        }

        private void BtnAccept_Click(object sender, EventArgs e)
        {
            var toast = Toast.MakeText(this, Resource.String.message_success_article_created, ToastLength.Short);
            toast.SetGravity(GravityFlags.Top | GravityFlags.CenterHorizontal, 0, 0);
            toast.Show();

            var inpNameArt = FindViewById<EditText>(Resource.Id.inpNameArticle);
            var inpDetailsArt = FindViewById<EditText>(Resource.Id.inpDetailsArticle);

            articleService.AddArticle(new Article() { Name = inpNameArt.Text, Details = inpDetailsArt.Text, Category = new Category { Name = categorySelected.Name } });

            Finish();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }
}
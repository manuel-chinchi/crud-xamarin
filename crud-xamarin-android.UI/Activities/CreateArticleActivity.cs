using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using crud_xamarin_android.Core.Models;
using crud_xamarin_android.Core.Services;
using crud_xamarin_android.UI.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crud_xamarin_android.UI.Activities
{
    [Activity(Label = "")]
    public class CreateArticleActivity : AppCompatActivity
    {
        Button btnAccept, btnCancel;
        Spinner spnCategories;

        ArticleService articleService;
        CategoryService categoryService;
        List<Category> categories;
        Category categorySelected;

        public CreateArticleActivity()
        {
            articleService = new ArticleService();
            categoryService = new CategoryService();
            categories = categoryService.GetCategories().ToList();
            categories = categories.OrderBy(c => c.Name).ToList();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_create_article);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            btnAccept = FindViewById<Button>(Resource.Id.btnAceptar);
            btnAccept.Click += BtnAccept_Click;

            btnCancel = FindViewById<Button>(Resource.Id.btnCancelar);
            btnCancel.Click += BtnCancel_Click;

            spnCategories = FindViewById<Spinner>(Resource.Id.spnCategories);
            var categories = categoryService.GetCategories().OrderBy(c=>c.Name).ToList();
            List<string> spnDataSource;
            ArrayAdapter spnAdapter;

            if (categories.Count > 0)
            {
                spnDataSource = categories.Select(c => c.Name).ToList();
            }
            else
            {
                spnDataSource = new List<string> { "(NO CATEGORIES)" };
                spnCategories.Enabled = false;
            }

            spnAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, spnDataSource);
            spnAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spnCategories.Adapter = spnAdapter;

            spnCategories.ItemSelected += SpnCategories_ItemSelected;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    OnBackPressed();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void SpnCategories_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (categories.Count > 0)
            {
                categorySelected = categories[e.Position];
            }
        }

        private void BtnAccept_Click(object sender, EventArgs e)
        {
            var toast = Toast.MakeText(this, Resource.String.message_success_article_created, ToastLength.Short);
            toast.SetGravity(GravityFlags.Top | GravityFlags.CenterHorizontal, 0, 0);
            toast.Show();

            var inpNameArt = FindViewById<EditText>(Resource.Id.inpNameArticle);
            var inpDetailsArt = FindViewById<EditText>(Resource.Id.inpDetailsArticle);

            var article = new Article
            {
                Name = inpNameArt.Text,
                Details = inpDetailsArt.Text,
                CategoryId = (categorySelected != null ? categorySelected.Id : 0)
            };
            articleService.AddArticle(article);

            SetResult(Result.Ok);

            Finish();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }
}
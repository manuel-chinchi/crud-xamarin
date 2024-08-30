using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using crud_xamarin_android.Core.Models;
using crud_xamarin_android.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crud_xamarin_android.UI.Activities
{
    [Activity(Label = "")]
    public class EditArticleActivity : AppCompatActivity
    {
        ArticleService articleService;
        Article article;
        EditText inpNameArticle;
        EditText inpDetailsArticle;
        Spinner spnCategory;
        Button btnAccept;
        Button btnCancel;
        List<Category> categories;

        public EditArticleActivity()
        {
            articleService = new ArticleService();
            categories = new CategoryService().GetCategories().ToList();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_create_article);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            inpNameArticle = FindViewById<EditText>(Resource.Id.inpNameArticle);
            inpDetailsArticle = FindViewById<EditText>(Resource.Id.inpDetailsArticle);
            spnCategory = FindViewById<Spinner>(Resource.Id.spnCategories);
            btnAccept = FindViewById<Button>(Resource.Id.btnAceptar);
            btnCancel = FindViewById<Button>(Resource.Id.btnCancelar);

            int articleId = Intent.GetIntExtra("ArticleId", -1);
            int categoryId = Intent.GetIntExtra("CategoryId", -1);
            article = articleService.GetArticleById(articleId);

            categories = categories.OrderBy(c => c.Name).ToList();
            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, categories.Select(c => c.Name).ToList());
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spnCategory.Adapter = adapter;
            if (article !=null)
            {
                inpNameArticle.Text = article.Name;
                inpDetailsArticle.Text = article.Details;

                int position = categories.FindIndex(c => c.Id == categoryId);
                spnCategory.SetSelection(position);
            }
            btnAccept.Click += BtnAccept_Click;
            btnCancel.Click += BtnCancel_Click;
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
        private void BtnAccept_Click(object sender, EventArgs e)
        {
            article.Name = inpNameArticle.Text;
            article.Details = inpDetailsArticle.Text;
            var category = categories[spnCategory.SelectedItemPosition];
            article.Category = categories.FirstOrDefault(c => c.Name == category.Name);
            article.CategoryId = category.Id;
            articleService.UpdateArticle(article);

            Intent resultIntent = new Intent();
            SetResult(Result.Ok, resultIntent);
            Finish();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            SetResult(Result.Canceled);
            Finish();
        }
    }
}
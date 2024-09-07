using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using crud_xamarin_android.Core.Helpers;
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
        EditText inpNameArticle;
        EditText inpDetailsArticle;
        Spinner spnCategory;
        Button btnAccept;
        Button btnCancel;

        Article article;
        ArticleService articleService;
        List<Category> categories;
        Category categorySelected;

        public EditArticleActivity()
        {
            articleService = new ArticleService();
            categories = new CategoryService().GetCategories().ToList();
            categories = categories.OrderBy(c => c.Name).ToList();
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
            ArrayAdapter spnAdapter;
            List<string> spnDataSource;

            if (article != null)
            {
                inpNameArticle.Text = article.Name;
                inpDetailsArticle.Text = article.Details;
            }

            if (categoryId == CategoryHelper.ID_EMPTY_CATEGORY && categories.Count > 0)
            {
                categories.Insert(0, new Category
                {
                    Id = CategoryHelper.ID_EMPTY_CATEGORY,
                    Name = CategoryHelper.NAME_EMPTY_CATEGORY
                });
                spnDataSource = categories.Select(c => c.Name).ToList();
                int position = categories.FindIndex(c => c.Id == categoryId);
                spnCategory.SetSelection(position);
            }
            else if (categories.Count > 0)
            {
                spnDataSource = categories.Select(c => c.Name).ToList();
                int position = categories.FindIndex(c => c.Id == categoryId);
                spnCategory.SetSelection(position);
            }
            else
            {
                spnDataSource = new List<string> { CategoryHelper.NAME_EMPTY_CATEGORY };
                spnCategory.Enabled = false;
            }

            spnAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, spnDataSource);
            spnAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spnCategory.Adapter = spnAdapter;

            btnAccept.Click += BtnAccept_Click;
            btnCancel.Click += BtnCancel_Click;
            spnCategory.ItemSelected += SpnCategory_ItemSelected;
        }

        private void SpnCategory_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (categories.Count > 0)
            {
                categorySelected = categories[e.Position];
            }
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

            if (categories.Count > 0)
            {
                article.Category = categories.FirstOrDefault(c => c.Name == categorySelected.Name);
                article.CategoryId = categorySelected.Id;
            }
            else
            {
                article.Category = null;
                article.CategoryId = CategoryHelper.ID_EMPTY_CATEGORY;
            }

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
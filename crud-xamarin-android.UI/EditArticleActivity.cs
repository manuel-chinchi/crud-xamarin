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
    [Activity(Label = "EditArticleActivity")]
    public class EditArticleActivity : Activity
    {
        ArticleService articleService;
        Article article;
        EditText inpNameArticle;
        EditText inpDetailsArticle;
        Button btnAccept;
        Button btnCancel;

        public EditArticleActivity()
        {
            articleService = new ArticleService();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_create_article);

            inpNameArticle = FindViewById<EditText>(Resource.Id.inpNameArticle);
            inpDetailsArticle = FindViewById<EditText>(Resource.Id.inpDetailsArticle);
            btnAccept = FindViewById<Button>(Resource.Id.btnAceptar);
            btnCancel = FindViewById<Button>(Resource.Id.btnCancelar);

            int articleId = Intent.GetIntExtra("ArticleId", -1);
            article = articleService.GetArticleById(articleId);
            if (article !=null)
            {
                inpNameArticle.Text = article.Name;
                inpDetailsArticle.Text = article.Details;
            }
            btnAccept.Click += BtnAccept_Click;
            btnCancel.Click += BtnCancel_Click;
        }

        private void BtnAccept_Click(object sender, EventArgs e)
        {
            article.Name = inpNameArticle.Text;
            article.Details = inpDetailsArticle.Text;

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
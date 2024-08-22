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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_create_article);

            var btnCancel = FindViewById<Button>(Resource.Id.btnCancelar);
            var btnAccept = FindViewById<Button>(Resource.Id.btnAceptar);

            btnAccept.Click += BtnAccept_Click;
            btnCancel.Click += BtnCancel_Click;
        }

        private void BtnAccept_Click(object sender, EventArgs e)
        {
            var toast = Toast.MakeText(this, "¡Articulo agregado exitosamente!", ToastLength.Short);
            toast.SetGravity(GravityFlags.Top | GravityFlags.CenterHorizontal, 0, 0);
            toast.Show();

            var inpNameArt = FindViewById<EditText>(Resource.Id.inpNameArticle);
            var inpDetailsArt = FindViewById<EditText>(Resource.Id.inpDetailsArticle);

            articleService.AddArticle(new Article() { Name = inpNameArt.Text, Details = inpDetailsArt.Text });

            Finish();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }
}
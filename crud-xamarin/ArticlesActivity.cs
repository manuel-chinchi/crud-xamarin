using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using crud_xamarin.Core.Models;
using crud_xamarin.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crud_xamarin
{
    [Activity(Label = "ArticlesActivity")]
    public class ArticlesActivity : Activity
    {
        RecyclerView recyclerView;
        ArticleAdapter adapter;
        List<Article> articles;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.articles);

            //Button btnGoBack = FindViewById<Button>(Resource.Id.btnGoBack);
            //btnGoBack.Click += delegate { StartActivity(typeof(MainActivity)); };

            //var lvwArticles = FindViewById<ListView>(Resource.Id.articleListView);
            var btnCreate = FindViewById<Button>(Resource.Id.btnAgregar);
            var btnEdit = FindViewById<Button>(Resource.Id.btnEditar);
            var btnDelete = FindViewById<Button>(Resource.Id.btnEliminar);

            recyclerView = FindViewById<RecyclerView>(Resource.Id.articleRecyclerView);
            recyclerView.SetLayoutManager(new LinearLayoutManager(this));



            var articleService = new ArticleService();
            //var articles = articleService.GetArticles();
            articles = articleService.GetArticles();

            //lvwArticles.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, articles.ConvertAll(a => a.Name));
            adapter = new ArticleAdapter(articles);
            recyclerView.SetAdapter(adapter);
        }
    }
}
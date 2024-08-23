using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using crud_xamarin_android.UI.Adapters;
using crud_xamarin_android.Core.Models;
using crud_xamarin_android.Core.Services;
using crud_xamarin_android.UI.Decorations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crud_xamarin_android.UI
{
    [Activity(Label = "ArticlesActivity")]
    public class ArticleActivity : Activity
    {
        RecyclerView recyclerView;
        ArticleAdapter adapter;
        ArticleService articleService;

        public ArticleActivity()
        {
            articleService = new ArticleService();
            adapter = new ArticleAdapter(articleService.GetArticles());
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_article);

            recyclerView = FindViewById<RecyclerView>(Resource.Id.lstArticles);

            #region old: drawing line separator for data grid

            //int color = Android.Graphics.Color.Gray;
            //int dividerHeight = 2;
            //recyclerView.AddItemDecoration(new GridItemDecoration(color, dividerHeight));

            #endregion

            recyclerView.SetLayoutManager(new LinearLayoutManager(this));
            //var articles = articleService.GetArticles();
            //adapter = new ArticleAdapter(articles);
            recyclerView.SetAdapter(adapter);

            var btnAdd = FindViewById<Button>(Resource.Id.btnAgregar);
            btnAdd.Click += BtnAdd_Click;

            var btnDelete = FindViewById<Button>(Resource.Id.btnEliminar);
            btnDelete.Click += BtnDelete_Click;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var positions = adapter.GetSelectedPositions();

            foreach (var pos in positions)
            {
                adapter.RemoveAt(pos);
            }

            adapter.ClearSelectedPositions();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(CreateArticleActivity));
            StartActivity(intent);
        }
    }
}
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
            btnDelete.Enabled = false;
            btnDelete.Click += BtnDelete_Click;

            var chkSelectAllItems = FindViewById<CheckBox>(Resource.Id.chkSelectAllItems);
            chkSelectAllItems.CheckedChange += ChkSelectAllItems_CheckedChange;
        }

        private void ChkSelectAllItems_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked)
            {
                adapter.SelectAllItems(true);
            }
            else
            {
                adapter.SelectAllItems(false);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var builder = new AlertDialog.Builder(this);
            builder.SetTitle("Delete");
            builder.SetMessage(Resource.String.message_warning_article_delete);
            builder.SetPositiveButton("Yes", (senderAlert, args) =>
            {
                DeleteArticle();
            });

            builder.SetNegativeButton("No", (senderAlert, args) =>
            {
                Toast.MakeText(this, Resource.String.message_cancel_generic, ToastLength.Short).Show();
            });

            var alertDialog = builder.Create();
            alertDialog.Show();
        }

        private void DeleteArticle()
        {
            var positions = adapter.GetSelectedPositions();

            foreach (var pos in positions)
            {
                adapter.RemoveAt(pos);
            }

            adapter.ClearSelectedPositions();
            ToggleDeleteButton(false);
            ToogleCheckHeader(false);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(CreateArticleActivity));
            StartActivity(intent);
        }

        public void ToggleDeleteButton(bool isAnySelected)
        {
            var btnDelete = FindViewById<Button>(Resource.Id.btnEliminar);
            btnDelete.Enabled = isAnySelected;
        }

        public void ToogleCheckHeader(bool isChecked)
        {
            var chkSelectAllItems = FindViewById<CheckBox>(Resource.Id.chkSelectAllItems);
            chkSelectAllItems.Checked = false;
        }
    }
}
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
using AndroidX.AppCompat.App;

namespace crud_xamarin_android.UI.Activities
{
    [Activity(Label = "Articles")]
    public class ArticleActivity : AppCompatActivity
    {
        Button btnAdd, btnEdit, btnDelete;
        CheckBox chkSelectAll;

        RecyclerView recyclerView;
        ArticleAdapter adapter;
        ArticleService articleService;

        public ArticleActivity()
        {
            articleService = new ArticleService();
            adapter = new ArticleAdapter(articleService.GetArticles().ToList());
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_article);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            recyclerView = FindViewById<RecyclerView>(Resource.Id.lstArticles);
            recyclerView.SetLayoutManager(new LinearLayoutManager(this));
            recyclerView.SetAdapter(adapter);

            btnAdd = FindViewById<Button>(Resource.Id.btnAgregar);
            btnAdd.Click += BtnAdd_Click;

            btnEdit = FindViewById<Button>(Resource.Id.btnEditar);
            btnEdit.Enabled = false;
            btnEdit.Click += BtnEdit_Click;

            btnDelete = FindViewById<Button>(Resource.Id.btnEliminar);
            btnDelete.Enabled = false;
            btnDelete.Click += BtnDelete_Click;

            chkSelectAll = FindViewById<CheckBox>(Resource.Id.chkSelectAllItems);
            chkSelectAll.CheckedChange += ChkSelectAllItems_CheckedChange;
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

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == 1 && resultCode == Result.Ok)
            {
                adapter.UpdateArticles(articleService.GetArticles().ToList());
                adapter.ClearSelectedPositions();
                adapter.NotifyDataSetChanged();
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
            }

            if (requestCode == 1000 && resultCode == Result.Ok)
            {
                adapter.UpdateArticles(articleService.GetArticles().ToList());
                adapter.NotifyDataSetChanged();
            }
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

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (adapter.GetSelectedPositions().Count == 1)
            {
                int position = adapter.GetSelectedPositions()[0];
                var article = adapter.GetArticleAt(position);

                var intent = new Intent(this, typeof(EditArticleActivity));
                intent.PutExtra("ArticleId", article.Id);
                // TODO check case 'null Category'!!
                intent.PutExtra("CategoryId", article.Category.Id);
                StartActivityForResult(intent, 1);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var builder = new AndroidX.AppCompat.App.AlertDialog.Builder(this);
            builder.SetTitle(Resource.String.title_delete);
            builder.SetMessage(Resource.String.message_warning_article_delete);
            builder.SetPositiveButton("Yes", (senderAlert, args) =>
            {
                DeleteArticle();
            });
            builder.SetNegativeButton("No", (senderAlert, args) =>
            {
                Toast.MakeText(this, Resource.String.message_cancel, ToastLength.Short).Show();
            });

            var alertDialog = builder.Create();
            alertDialog.Show();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(CreateArticleActivity));
            StartActivityForResult(intent, 1000);
        }

        private void DeleteArticle()
        {
            var positions = adapter.GetSelectedPositions();

            foreach (var pos in positions)
            {
                articleService.DeleteArticle(adapter.GetArticleAt(pos).Id);
                adapter.RemoveAt(pos);
            }

            adapter.UpdateArticles(articleService.GetArticles().ToList());
            adapter.ClearSelectedPositions();
            ToggleDeleteButton(false);
            ToogleCheckHeader(false);
        }

        public void ToggleDeleteButton(bool isAnySelected)
        {
            btnDelete.Enabled = isAnySelected;
        }

        public void ToggleEditButton(bool isOneItemSelected)
        {
            btnEdit.Enabled = isOneItemSelected;
        }

        private void ToogleCheckHeader(bool isChecked)
        {
            chkSelectAll.Checked = false;
        }
    }
}
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
    [Activity(Label = "Categories")]
    public class CategoryActivity : AppCompatActivity
    {
        Button btnAdd, btnDelete;
        CheckBox chkSelectAll;

        RecyclerView recyclerView;
        CategoryAdapter adapter;
        CategoryService categoryService;

        public CategoryActivity()
        {
            categoryService = new CategoryService();
            adapter = new CategoryAdapter(categoryService.GetCategories().ToList());
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_category);
            
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            recyclerView = FindViewById<RecyclerView>(Resource.Id.lstCategories);
            recyclerView.SetLayoutManager(new LinearLayoutManager(this));
            recyclerView.SetAdapter(adapter);

            btnAdd = FindViewById<Button>(Resource.Id.btnAddCategory);
            btnAdd.Click += BtnAddCategory_Click;

            btnDelete = FindViewById<Button>(Resource.Id.btnDeleteCategory);
            btnDelete.Enabled = false;
            btnDelete.Click += BtnDeleteCategory_Click;

            chkSelectAll = FindViewById<CheckBox>(Resource.Id.chkSelectAllCategories);
            chkSelectAll.CheckedChange += ChkSelectAllCategories_CheckedChange;
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

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == 1000 && resultCode ==Result.Ok)
            {
                adapter.UpdateCategories(categoryService.GetCategories().ToList());
                adapter.NotifyDataSetChanged();
            }
        }

        private void ChkSelectAllCategories_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
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

        private void BtnAddCategory_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(CreateCategoryActivity));
            StartActivityForResult(intent, 1000);
        }

        private void BtnDeleteCategory_Click(object sender, EventArgs e)
        {
            var builder = new AndroidX.AppCompat.App.AlertDialog.Builder(this);
            builder.SetTitle(Resource.String.title_delete);
            builder.SetMessage(Resource.String.message_delete_category);
            builder.SetPositiveButton("Yes", (senderAlert, args) =>
            {
                ConfirmOrCancelDeleteCategory();
            });
            builder.SetNegativeButton("No", (senderAlert, args) =>
            {
                Toast.MakeText(this, Resource.String.message_cancel , ToastLength.Short).Show();
            });

            var alertDialog = builder.Create();
            alertDialog.Show();
        }

        private void ConfirmOrCancelDeleteCategory()
        {
            bool hasRelatedArticles = false;
            var positions = adapter.GetSelectedPositions();

            foreach (var pos in positions)
            {
                var category = (Category)adapter.GetItemAt(pos);
                if (category.ArticleCount != 0)
                {
                    hasRelatedArticles = true;
                    continue;
                }
            }

            if (hasRelatedArticles)
            {
                var builder = new AndroidX.AppCompat.App.AlertDialog.Builder(this);
                builder.SetTitle(Resource.String.title_warning);
                builder.SetMessage(Resource.String.message_warning_delete_category);
                builder.SetPositiveButton("Yes", (sender, args) =>
                {
                    DeleteCategory();
                });
                builder.SetNegativeButton("No", (sender, args) =>
                {
                });

                var alertDialog = builder.Create();
                alertDialog.Show();
            }
            else
            {
                DeleteCategory();
            }
        }

        private void DeleteCategory()
        {
            var positions = adapter.GetSelectedPositions();

            foreach (var pos in positions)
            {
                categoryService.DeleteCategory(((Category)adapter.GetItemAt(pos)).Id);
                adapter.RemoveAt(pos);
            }

            adapter.UpdateCategories(categoryService.GetCategories().ToList());
            adapter.ClearSelectedPositions();
            ToogleDeleteButton(false);
            ToogleCheckHeader(false);
        }

        public void ToogleDeleteButton(bool isAnySelected)
        {
            btnDelete.Enabled = isAnySelected;
        }

        public void ToogleCheckHeader(bool isChecked)
        {
            chkSelectAll.Checked = isChecked;
        }
    }
}
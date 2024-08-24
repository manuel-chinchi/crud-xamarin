using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using crud_xamarin_android.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crud_xamarin_android.UI.Adapters
{
    public class CategoryAdapter : RecyclerView.Adapter
    {
        List<Category> categories;
        public CategoryAdapter(List<Category> categories)
        {
            this.categories = categories;
        }


        public override int ItemCount => categories.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = holder as CategoryViewHolder;
            viewHolder.Id.Text = categories[position].Id.ToString();
            viewHolder.Name.Text = categories[position].Name;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_category, parent, false);
            return new CategoryViewHolder(view);
        }
    }

    public class CategoryViewHolder : RecyclerView.ViewHolder
    {
        public TextView Id { get; private set; }
        public TextView Name { get; private set; }
        public CategoryViewHolder(View itemView) : base(itemView)
        {
            Id = ItemView.FindViewById<TextView>(Resource.Id.colIdCategory);
            Name = ItemView.FindViewById<TextView>(Resource.Id.colNameCategory);
        }
    }
}